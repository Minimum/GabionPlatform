var LPContent = {};

LPContent.GetContent = function(id, callback) {
    $.getJSON(LanPlatform.ApiPath + "content/view/id/" + id, {}, callback);

    return;
}

LPContent.InitializeContent = function(model) {
    var content = new LPContent.ContentItem();

    content.LoadModel(model);

    return content;
}

LPContent.UploadContent = function(data) {
    
}