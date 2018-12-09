
'use strict';

var ws = require('ws');
var UUID = require('node-uuid');
var events = require('events');
var util = require('util');
var crypto = require('crypto');
var debug = require('./debug');

const WebSocket = require('ws');

exports = module.exports = {};

var ApiHost = "openapi.huya.com";
//ApiHost = "127.0.0.1:8080";

function getTimestamp() {
    return Math.floor(Date.now() / 1000);
}

function HuyaHelper() {
    this.isWss = true;
    this.heartbeat_ms = 15000;
    this.timeout_ms = 30000;
    this.danmuSocket = null;
    this.giftSocket = null;
    this.danmuPingInterval = null;
    this.danmuPingTimeout = null;
    this.giftPingInterval = null;
    this.giftPingTimeout = null;
}

HuyaHelper.prototype.danmuHeartbeat = function() {
    var that = this;
    if (that.danmuPingTimeout != null)
        clearTimeout(that.danmuPingTimeout);

    if (that.danmuPingInterval == null) {
        debug.print(">>>> HuyaHelper::danmuHeartbeat() this.danmuPingInterval create.");
        that.danmuSocket.send('ping');
        that.danmuPingInterval = setInterval(function ping() {
            //debug.print("this.danmuSocket.readyState = ", that.danmuSocket.readyState);
            if (that.danmuSocket.readyState == WebSocket.OPEN) {
                that.danmuSocket.send('ping');
            }
            else if (that.danmuSocket.readyState == WebSocket.CLOSED) {
                clearInterval(that.danmuPingInterval);
                that.danmuPingInterval = null;
            }
        }, that.heartbeat_ms);
    }

    /*
    // Use `WebSocket#terminate()` and not `WebSocket#close()`. Delay should be
    // equal to the interval at which your server sends out pings plus a
    // conservative assumption of the latency.
    that.danmuPingTimeout = setTimeout(() => {
        debug.print("this.danmuSocket.readyState = ", that.danmuSocket.readyState);
        if (that.danmuSocket.readyState == WebSocket.OPEN)
            that.danmuSocket.terminate();
    }, that.timeout_ms + 1000);
    //*/
}

HuyaHelper.prototype.giftHeartbeat = function() {
    var that = this;
    if (that.giftPingTimeout != null)
        clearTimeout(that.giftPingTimeout);

    if (that.giftPingInterval == null) {
        debug.print(">>>> HuyaHelper::giftHeartbeat() this.giftPingInterval create.");
        that.giftSocket.send('ping');
        that.giftPingInterval = setInterval(function ping() {
            //debug.print("this.giftSocket.readyState = ", that.giftSocket.readyState);
            if (that.giftSocket.readyState == WebSocket.OPEN) {
                that.giftSocket.send('ping');
            }
            else if (that.giftSocket.readyState == WebSocket.CLOSED) {
                clearInterval(that.giftPingInterval);
                that.giftPingInterval = null;
            }
        }, that.heartbeat_ms);
    }

    /*
    // Use `WebSocket#terminate()` and not `WebSocket#close()`. Delay should be
    // equal to the interval at which your server sends out pings plus a
    // conservative assumption of the latency.
    that.giftPingTimeout = setTimeout(() => {
        debug.print("this.giftSocket.readyState = ", that.giftSocket.readyState);
        if (that.giftSocket.readyState == WebSocket.OPEN)
            that.giftSocket.terminate();
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

    var timestamp = getTimestamp();
    var data = "{\"roomId\":" + roomId + "}";
    var md5 = crypto.createHash('md5');
    var sign_orig = 'data=' + data + '&key=' + secretId + '&timestamp=' + timestamp;
    var sign = md5.update(sign_orig).digest('hex');

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
        that.danmuHeartbeat();
        debug.leave("danmuSocket::on_open()");
    });

    that.danmuSocket.on('ping', function ping() {
        debug.enter("danmuSocket::on_ping()");
        //that.danmuHeartbeat();
        //that.danmuSocket.pong('pong');
        debug.leave("danmuSocket::on_ping()");
    });

    that.danmuSocket.on('close', function clear() {
        debug.enter("danmuSocket::on_close()");
        if (this.danmuPingInterval != null) {
            clearInterval(that.danmuPingInterval);
            that.pingInterval = null;
        }
        if (this.danmuPingTimeout != null) {
            clearTimeout(that.danmuPingTimeout);
            that.danmuPingTimeout = null;
        }
        debug.leave("danmuSocket::on_close()");
    });
    
    // Processing the received message.
    that.danmuSocket.on('message', function incoming(data) {
        debug.enter("danmuSocket::on_message()");

        debug.print("typeof(data) = " + typeof(data));
        debug.print('on_message: ', data);

        var json_str;
        var dataType = typeof(data);        
        if (dataType != "Object") {
            json_str = data;
        }
        else {
            json_str = JSON.stringify(data)
            //debug.print("json = " + json_str);
        }

        if (json_str == "pong") {
            console.log("timestamp: %d, danmu pong.", getTimestamp());
        }
        else {
            var json = JSON.parse(json_str);
            if (json.statusCode == 200) {
                // TODO: processing danmu data: json.data
                console.log('[' + json.data.sendNick + ']: ' + json.data.content + '');
            }
            else {
                // TODO: Error handler
                console.log("Error: statusCode = " + json.statusCode);
            }
        }

        debug.leave("danmuSocket::on_message()");
    });

    debug.leave("HuyaHelper::loginDanmu()");
}

HuyaHelper.prototype.loginGift = function (appId, secretId, roomId) {
    var that = this;
    debug.enter("HuyaHelper::loginGift()");

    var timestamp = getTimestamp();
    var data = "{\"roomId\":" + roomId + "}";
    var md5 = crypto.createHash('md5');
    var sign_orig = 'data=' + data + '&key=' + secretId + '&timestamp=' + timestamp;
    var sign = md5.update(sign_orig).digest('hex');

    var apiUrl;
    if (that.isWss)
        apiUrl = "wss://";
    else
        apiUrl = "ws://";
    apiUrl += ApiHost + "/index.html?do=getSendItemNotice&data=" + data + "&appId=" + appId + "&timestamp=" + timestamp + "&sign=" + sign;

    debug.print("apiUrl = " + apiUrl);

    try {
        that.giftSocket = new WebSocket(apiUrl, {
            perMessageDeflate: false
        });
    }
    catch (ex) {
        debug.print("exception: ", ex);
    }

    that.giftSocket.on('open', function open() {
        debug.enter("giftSocket::on_open()");
        that.giftHeartbeat();
        debug.leave("giftSocket::on_open()");
    });

    that.giftSocket.on('ping', function ping() {
        debug.enter("giftSocket::on_ping()");
        //that.giftHeartbeat();
        //that.giftSocket.pong('pong');
        debug.leave("giftSocket::on_ping()");
    });

    that.giftSocket.on('close', function clear() {
        debug.enter("giftSocket::on_close()");
        if (this.giftPingInterval != null) {
            clearInterval(that.giftPingInterval);
            that.giftPingInterval = null;
        }
        if (this.giftPingTimeout != null) {
            clearTimeout(that.giftPingTimeout);
            that.giftPingTimeout = null;
        }
        debug.leave("giftSocket::on_close()");
    });
    
    // Processing the received message.
    that.giftSocket.on('message', function incoming(data) {
        debug.enter("giftSocket::on_message()");

        debug.print("typeof(data) = " + typeof(data));
        debug.print('on_message: ', data);

        var json_str;
        var dataType = typeof(data);        
        if (dataType != "Object") {
            json_str = data;
        }
        else {
            json_str = JSON.stringify(data)
            //debug.print("json = " + json_str);
        }

        if (json_str == "pong") {
            console.log("timestamp: %d, gift pong.", getTimestamp());
        }
        else {
            var json = JSON.parse(json_str);
            if (json.statusCode == 200) {
                // TODO: processing gift data: json.data
                console.log('[' + json.data.sendNick + ']: \"' + json.data.itemName + '\" x ' + json.data.sendItemCount);
            }
            else {
                // TODO: Error handler
                console.log("Error: statusCode = " + json.statusCode);
            }
        }

        debug.leave("giftSocket::on_message()");
    });

    debug.leave("HuyaHelper::loginGift()");
}

module.exports.create = function() {
    var helper = new HuyaHelper();
    return helper
}
