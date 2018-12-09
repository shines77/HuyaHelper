
'use strict';

var HuyaHelper = require('./HuyaHelper');

console.log("");
console.log("Welcome to HuyaHelper v1.0");
console.log("");

var huyaHelper = HuyaHelper.create();

huyaHelper.loginDanmu("15405257255989910", "1efb490c", 15382773);
huyaHelper.loginGift("15405257255989910", "1efb490c", 15382773);
