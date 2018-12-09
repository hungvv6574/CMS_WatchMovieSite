function initSliderOption(id) {
    //var _SlideshowTransitions = [{ $Duration: 1200, $Opacity: 2 }];

    var options = {
        $AutoPlay: false,
        $AutoPlaySteps: 1,
        $AutoPlayInterval: 5000,
        $PauseOnHover: 1,

        $ArrowKeyNavigation: true,
        $SlideDuration: 500,
        $MinDragOffsetToSlide: 20,
        //$SlideWidth: 600,                        
        //$SlideHeight: 300,                       
        $SlideSpacing: 0,
        $DisplayPieces: 1,
        $ParkingPosition: 0,
        $UISearchMode: 1,
        $PlayOrientation: 1,
        $DragOrientation: 3,

        //$SlideshowOptions: {
        //    $Class: $JssorSlideshowRunner$,
        //    $Transitions: _SlideshowTransitions,
        //    $TransitionsOrder: 1,
        //    $ShowLink: true
        //},

        $BulletNavigatorOptions: {
            $Class: $JssorBulletNavigator$,
            $ChanceToShow: 2,
            $AutoCenter: 1,
            $Steps: 1,
            $Lanes: 1,
            $SpacingX: 10,
            $SpacingY: 10,
            $Orientation: 1
        },

        $ArrowNavigatorOptions: {
            $Class: $JssorArrowNavigator$,
            $ChanceToShow: 2,
            $Steps: 1
        }
    };
    
    $JssorSlider$(id, options);
}
 
function event_mouseover() {
    $(".caption").addClass("display");
}

function event_mouseout() {
    $(".caption").removeClass("display");
}

function getUrlParam(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return unescape(results[1]);
}

function addQueryStringParameter(uri, key, value) {
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = uri.indexOf('?') !== -1 ? "&" : "?";
    if (uri.match(re)) {
        return uri.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
        return uri + separator + key + "=" + value;
    }
}

function convertObj(ix) {
    var dx = {};
    dx = ix;
    return dx;
}