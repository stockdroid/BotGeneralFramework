/// <reference path="./types.d.ts" />
app.on("ready", (ctx, next) => {
  console.log(config.options.envVar);
  return next();
});