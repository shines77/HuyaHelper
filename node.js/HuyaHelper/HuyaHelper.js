
'use strict';

var ws = require('ws');
var UUID = require('node-uuid');
var events = require('events');
var util = require('util');
var crypto = require('crypto');
var debug = require('./debug');

const WebSocket = require('ws');

exports = module.exports = {};

function getTimestamp() {
    return Math.floor(Date.now() / 1000);
}

function HuyaHelper() {
    this.isWss = false;
    this.heartbeat_ms = 15000;
    this.timeout_ms = 30000;
    this.pingInterval = null;
    this.pingTimeout = null;
    this.danmuSocket = null;
    this.giftSocket = null;
}

HuyaHelper.prototype.heartbeat = function() {
    var that = this;
    if (that.pingTimeout != null)
        clearTimeout(that.pingTimeout);

    if (that.pingInterval == null) {
        debug.print(">>>> HuyaHelper::heartbeat() this.pingInterval create.");
        that.danmuSocket.send('ping');
        that.pingInterval = setInterval(function ping() {
            debug.print("this.danmuSocket.readyState = ", that.danmuSocket.readyState);
            if (that.danmuSocket.readyState == WebSocket.OPEN) {
                that.danmuSocket.send('ping');
            }
            else if (that.danmuSocket.readyState == WebSocket.CLOSED) {
                clearInterval(that.pingInterval);
                that.pingInterval = null;
            }
        }, that.heartbeat_ms);
    }

    /*
    // Use `WebSocket#terminate()` and not `WebSocket#close()`. Delay should be
    // equal to the interval at which your server sends out pings plus a
    // conservative assumption of the latency.
    that.pingTimeout = setTimeout(() => {
        debug.print("this.danmuSocket.readyState = ", that.danmuSocket.readyState);
        if (that.danmuSocket.readyState == WebSocket.OPEN)
            that.danmuSocket.terminate();
    }, that.timeout_ms + 1000);
    //*/
}

HuyaHelper.prototype.terminate = function() {
    var that = this;
    debug.enter("HuyaHelper::terminate()");
    debug.leave("HuyaHelper::terminate()");
}

HuyaHelper.prototype.loginDanmu = function (appId, secretId, roomId) {
    var that = this;
    debug.enter("HuyaHelper::loginDanmu()");

    var ApiHost = "openapi.huya.com";
    ApiHost = "127.0.0.1:8080";

    var data = "{\"roomId\":" + roomId + "}";
    var md5 = crypto.createHash('md5');
    var sign_orig = 'data=' + data + '&key=' + secretId + '&timestamp=' + timestamp;
    var sign = md5.update(sign_orig).digest('hex');

    var timestamp = getTimestamp();
    var apiUrl;
    if (that.isWss)
        apiUrl = "wss://";
    else
        apiUrl = "ws://";
    apiUrl += ApiHost + "/index.html?do=getMessageNotice&data=" + data + "&appId=" + appId + "&timestamp=" + timestamp + "&sign=" + sign;

    debug.print("apiUrl = " + apiUrl);

    try {
        that.danmuSocket = new WebSocket(apiUrl, {
            perMessageDeflate: false
        });
    }
    catch (ex) {
        debug.print("exception: ", ex);
    }

    that.danmuSocket.on('open', function open() {
        debug.enter("danmuSocket::on_open()");
        that.heartbeat();
        debug.leave("danmuSocket::on_open()");
    });

    that.danmuSocket.on('ping', function ping() {
        debug.enter("danmuSocket::on_ping()");
        //that.heartbeat();
        //that.danmuSocket.pong('pong');
        debug.leave("danmuSocket::on_ping()");
    });

    that.danmuSocket.on('close', function clear() {
        debug.enter("danmuSocket::on_close()");
        if (this.pingInterval != null) {
            clearInterval(that.pingInterval);
            that.pingInterval = null;
        }
        if (this.pingTimeout != null) {
            clearTimeout(that.pingTimeout);
            that.pingTimeout = null;
        }
        debug.leave("danmuSocket::on_close()");
    });
    
    // 处理到来的信令
    that.danmuSocket.on('message', function incoming(data) {
        debug.enter("danmuSocket::on_message()");

        debug.print("typeof(data) = " + typeof(data));
        debug.print('on_message: ', data);

        var dataType = typeof(data);

        if (dataType == "string") {
            if (data == "pong") {
                //debug.print("timestamp: " + getTimestamp() + ", pong.");
                debug.print("timestamp: %d, pong.", getTimestamp());
            }
        }
        else if (dataType == "Object") {
            var jsonStr = JSON.stringify(data)
            debug.print("json = " + jsonStr);

            var json = JSON.parse(data);
            if (json.statusCode == 200) {
                // TODO: 处理弹幕数据json.data
                debug.print('[' + json.data.sendNick + ']: ' + json.data.content + '');
            }
            else {
                // TODO: 错误处理
                debug.print("Error: statusCode = " + json.statusCode);
            }
        }

        debug.leave("danmuSocket::on_message()");
    });

    debug.leave("HuyaHelper::loginDanmu()");
}

module.exports.create = function() {
    var helper = new HuyaHelper();
    return helper
}
