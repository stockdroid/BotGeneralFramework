/// <reference path="../types.d.ts" />
const { assertCommandName } = require("../utils.js");

/**
 * @param {App} app
 */
exports = (app) =>
  app.on("message", (ctx, next) => {
    if (!assertCommandName(ctx.message.text, "ping")) return next();
    ctx.replyMsg.edit("PONG!");
  });