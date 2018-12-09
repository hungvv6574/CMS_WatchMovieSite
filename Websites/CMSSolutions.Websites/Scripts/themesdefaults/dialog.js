function dialogBox(id) {
    $(id).fadeIn("slow");
    $('body').append('<div id="over"></div>');
    $('#over').fadeIn(300);
}

$(document).ready(function () {
    $(document).on('click', "a.close-reveal-modal, #over", function () {
        $('#over, .reveal-modal').fadeOut(300, function () {
            $('#over').remove();  
        });
        
        return false;
    });
});


