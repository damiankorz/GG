$(document).ready(function(){
    // underline links 
    $('.links').mouseenter(function(){
        $(this).css("text-decoration", "underline")
    })
    $('.links').mouseout(function(){
        $(this).css("text-decoration", "none")
    })
    // change color and background of links 
    $('.button_links').mouseenter(function(){
        $(this).css("color", "black")
        $(this).css("background", "silver")
    })
    $('.button_links').mouseout(function(){
        $(this).css("color", "silver")
        $(this).css("background", "linear-gradient(to bottom right, black, #4d4e4e)")
    })
    // change color and background color of button on login and register
    $('form button').mouseenter(function(){
        $(this).css("background", "silver")
        $(this).css("color", "black")
    })
    $('form button').mouseout(function(){
        $(this).css("background", "linear-gradient(to bottom right, black, #4d4e4e)")
        $(this).css("color", "silver")
    })
    // change color of nav links and nav button background
    $('.nav_links').mouseenter(function(){
        $(this).css("color", "silver")
        $(this).css("background", "black")
    })
    $('.nav_links').mouseout(function(){
        $(this).css("color", "black")
        $(this).css("background", "transparent")
    })
    //extend body to document height
    $('body').height($(document).height());
    //extend html to document width
    $('html').width($(document).width());
});
