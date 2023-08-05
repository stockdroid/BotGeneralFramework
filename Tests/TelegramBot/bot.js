/// <reference path="./types.d.ts" />
/// <reference path="./globals.d.ts" />

app.on("ready", (_ctx, next) => {
  console.log("Bot is ready");
  return next();
});

app.on("telegram.ready", (_ctx, next) => {
  console.log("Telegram bot has been set up");
  return next();
});
app.on("telegram.terminated", (_ctx, next) => {
  console.log("Telegram bot instance terminated");
  return next();
});

app.on("telegramTaskQueue.ready", (_ctx, next) => {
  console.log("Telegram queue ready");
  return next();
});
app.on("telegramTaskQueue.elementEnqueued", ({ queueCount }, next) => {
  console.log("Telegram queue updated: added event");
  console.log(` queue has now ${queueCount} events`);
});
app.on("telegramTaskQueue.elementDequeued", ({ queueCount }, next) => {
  console.log("Telegram queue updated: removed event");
  console.log(` queue has now ${queueCount} events`);
});

require("./commands/info.js")(app, config);
require("./commands/ping.js")(app);
require("./commands/default.js")(app);

require("./cli/info.js")(app, config);
require("./cli/config.js")(app, config, options);
require("./cli/evaluate.js")(app, config, options);