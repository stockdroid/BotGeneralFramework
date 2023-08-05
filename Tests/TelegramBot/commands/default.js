/// <reference path="../types.d.ts" />

/**
 * @param {App} app
 */
exports = (app) =>
  app.on("message", (ctx, next) => {
    ctx.replyMsg.edit("❌ Command not recognized!");
  });