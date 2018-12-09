
var util = require('util');

var debug = exports = module.exports = {};

debug.is_debug = false;

debug.setDebug = function setDebug(is_debug) {
    debug.is_debug = is_debug;
}

debug.print = function print(format, ...params) {
    if (debug.is_debug) {
        var text = util.format(format, ...params);
        console.log(text);
    }
}

debug._print = function _print(format, ...params) {
    if (debug.is_debug) {
        if (true || format.indexOf("%") == -1) {
            var text = util.format(format, ...params);
            util.print(text);
            util.print("\n");
        }
        else {
            var args = Array.prototype.slice.call(arguments);
            var num_args = arguments.length;
            for (i = 0; i < args.length; i++) {
                util.print(args[i]);
            }
            util.print("\n");
        }
    }
}

debug.enter = function enter(funcName) {
    if (debug.is_debug) {
        console.log("-------------------------------------------");
        console.log(funcName + " enter.");
    }
}

debug.leave = function leave(funcName) {
    if (debug.is_debug) {
        console.log(funcName + " leave.");
        console.log("-------------------------------------------");
    }
}

function createDebug() {
    var debug = new Debug();
    return debug;
}
