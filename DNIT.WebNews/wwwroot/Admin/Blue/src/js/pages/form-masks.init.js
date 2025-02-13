/*
Template Name: Abstack - Bootstrap 4 Web App kit
Author: CoderThemes
File: Form mask init js
*/

$( document ).ready(function() {
    $('[data-toggle="input-mask"]').each(function (idx, obj) {
        var maskFormat = $(obj).data("maskFormat");
        var reverse = $(obj).data("reverse");
        if (reverse != null)
            $(obj).mask(maskFormat, {'reverse': reverse});
        else
            $(obj).mask(maskFormat);
    });
});

jQuery(function($) {
    $('.autonumber').autoNumeric('init');
});
