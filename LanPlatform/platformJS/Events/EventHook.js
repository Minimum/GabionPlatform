LPEvents.EventHook = function(handler, handleId, callback) {
    this.Handler = handler;
    this.HandleId = handleId;
    this.Callback = callback;
}

LPEvents.EventHook.prototype.RemoveHook = function() {
    this.Handler.RemoveHook(this);

    return;
}

LPEvents.EventHook.prototype.Invoke = function(sender, args) {
    this.Callback(sender, args);

    return;
}