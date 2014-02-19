function onSourceDownloadProgressChanged(sender, eventArgs) {    
    var val = Math.round((eventArgs.progress * 1000)) / 10;
    sender.findName("progressText").Text = String(Math.round(val));
    sender.findName("ProgressBarTransform").ScaleX = val / 100;
}