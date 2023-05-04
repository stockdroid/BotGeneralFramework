# App

This section analyses in depth the 'app' object.

## Introduction

The app object is referenced in the main script of the project and cannot be accessed from other modules.

## Object structure

The object is structured into many functions that can be called sequentially.
```javascript
app.use((ctx, next) => console.log("middleware called"))
   .on("ready", (ctx, next) => console.log("bot ready!"));
```

### Use

#### Definition

```javascript
app.use(
  middleware: (ctx: Context, next: () => Promise<any>) => Promise<any>
)
```

#### Description

The `use` function registers a middleware that will be called on any triggered event before the corresponding handlers.

### On

#### Definition

```javascript
app.on(
  eventName: string,
  handler: (ctx: Context, next: () => Promise<any>) => Promise<any>
)
```

#### Description

The `on` function registers an handler that will be triggered when the event specified is fired by the application.

#### Events

##### ready
The **ready** event is fired when the BotGF application instance is ready and listening for events on the specified platforms
```javascript
app.on("ready", (ctx, next) => {
  // perform some setup operatons
  return next();
});
```

##### [platformName].ready
This event is fired when a particular platform is ready and listening for updates.
```javascript
app.on("telegram.ready", (ctx, next) => {
  // perform some setup operatons
  return next();
});
```
The `ctx` parameter contains:
  - bot: It is the [bot instance](./bot.md)
  - config: It is the platform information used to initialize the current platform

##### [platformName].terminated
This event is fired when a particular platform is terminated and has stopped listening for updates.
```javascript
app.on("telegram.terminated", (ctx, next) => {
  // perform some cleanup operatons
  return next();
});
```

##### message
This event is triggered when one of the platforms listening to updates receives a new message
```javascript
app.on("message", (ctx, next) => {
  // perform some operations
  return next();
});
```
The `ctx` parameter contains:
  - bot: It is the [bot instance](./bot.md)
  - message: It is the [message](./message.md) that triggered the event
  - replyMsg: It is the [message](./message.md) that was sent while waiting the bot response, it might be null for some platforms