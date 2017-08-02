LPEvents.EventHandler = function() {
    this.Hooks = [];
    this.NewHookId = 0;
}

LPEvents.EventHandler.prototype.AddHook = function (callback) {
    var hook = new LPEvents.EventHook(this, this.NewHookId, callback);

    this.Hooks[this.NewHookId] = hook;

    this.NewHookId++;
    
    return hook;
}

LPEvents.EventHandler.prototype.RemoveHook = function(hook) {
    this.Hooks[hook.HandleId] = null;

    return;
}

LPEvents.EventHandler.prototype.Invoke = function(sender, args) {
    this.Hooks.forEach(function(hook) {
        hook.Invoke(sender, args);
    });

    return;
}