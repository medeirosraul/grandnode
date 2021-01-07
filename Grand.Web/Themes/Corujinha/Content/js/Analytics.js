// Wait document ready
$(document).ready(() => {
    TrackNavigation();
});

// Prepare user track id
let owl_user_track_id = localStorage.getItem('owl_user_track_id');
if (owl_user_track_id == undefined || owl_user_track_id == '') {
    owl_user_track_id = createGuid();
    localStorage.setItem('owl_user_track_id', owl_user_track_id);
}

// Events to track
TrackNavigation = () => {
    $.ajax({
        cache: false,
        type: "POST",
        url: "/Analytics/Track",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify( {
            DateTime: new Date(),
            Session: owl_user_track_id,
            Code: 'Event.Navigation',
            Parameters: [
                { Key: 'Url', Value: window.location.href }
            ]
        }),
        complete: function () {
            console.log('event trackeds')
        }
    });
}

// Utilities
function createGuid() {
    function S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }
    return (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
} 