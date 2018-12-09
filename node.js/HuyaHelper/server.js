
'use strict';

const fs = require('fs');
const http = require('http');
const WebSocket = require('ws');

const server = new http.createServer();
const wss = new WebSocket.Server({ server });

this.heartbeat_ms = 15000;
this.pingInterval = null;

function getTimestamp() {
    return Math.floor(Date.now() / 1000);
}

function enterFunction(funcName) {
    console.log("-------------------------------------------");
    console.log(funcName + " Enter.");
}

function leaveFunction(funcName) {
    console.log(funcName + " Leave.");
    console.log("-------------------------------------------");
}

function makeResponse(statusCode, statusMsg, data) {
    var json = "{ ";
    json += "\"statusCode\": " + statusCode + ",";
    json += "\"statusMsg\": \"" + statusMsg + "\",";
    json += "\"data\": " + data;
    json += " }";
    return json;
}

function makeData(roomId, sendNick, content) {
    var json = "{ ";
    json += "\"roomId\": " + roomId + ",";
    json += "\"sendNick\": \"" + sendNick + "\",";
    json += "\"content\": \"" + content + "\"";
    json += " }";
    return json;
}

wss.on('connection', function connection(ws) {
    var that = this;
    enterFunction("connection()");

    // console.log("host: %s, port: %d", server.address().address, server.address().port);
    ws.on('message', function incoming(message) {
        console.log('on_message(): ' + getTimestamp() + ' received data: \"%s\"', message);
        if (typeof(message) == "string") {
            if (message == "ping") {
                if (ws.readyState == WebSocket.OPEN) {
                    ws.send("pong");
                }
            }
        }
    });

    ws.on('ping', function pong(message) {
        console.log('on_ping(): ' + getTimestamp() + 'received data: %s', message);
        //ws.send('pong');
    });

    ws.on('pong', function recevive_pong(message) {
        console.log('on_pong(): ' + getTimestamp() + 'received data: %s', message);
        //var data = makeData(15382773, "shines77", "ping");
        //var response = makeResponse(200, "", data);
        //console.log("response = " + response);
        //ws.send(response);
    });

    if (ws.pingInterval == null) {
        console.log(">>>> ws.pingInterval create.")
        ws.pingInterval = setInterval(function ping() {
            if (ws.readyState == WebSocket.OPEN) {
                //ws.ping('ping');
            }
            else {
                clearInterval(ws.pingInterval);
                ws.pingInterval = null;
            }
        }, that.heartbeat_ms);
    }

    var data = makeData(15382773, "shines77", "Hello world");
    var response = makeResponse(200, "", data);
    console.log("response = " + response);
    ws.send(response);

    leaveFunction("connection()");
});

wss.on('ping', function ping(ws) {
    enterFunction("ping()");

    ws.on('ping', function pong(message) {
        console.log('on_ping(): ' + getTimestamp() + 'received data: %s', message);
    });

    ws.send('pong');
    
    leaveFunction("ping()");
});

console.log("");
console.log("server running ...");
console.log("");
server.listen(8080, "127.0.0.1");
