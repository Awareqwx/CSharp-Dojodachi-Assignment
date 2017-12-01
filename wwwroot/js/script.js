$(document).ready(function(){
    initialize();
    $("button[type='button']").click(function(){
        $.get("/" + $(this).attr("value"), function(res)
        {
            let full = res.stats.value.fullness;
            let happ = res.stats.value.happiness
            let nrg = res.stats.value.energy;
            $("#fullness").html(full);
            $("#happiness").html(happ);
            $("#meals").html(res.stats.value.meals);
            $("#energy").html(nrg);
            $("#message").html(res.message);
            $("img").attr("src", "/images/" + res.image + ".jpg");
            if(full == 0 || happ == 0)
            {
                $("button[type='button']").hide();
                $("button[type='submit']").show();
                $("#message").html("Your Dojodachi has passed away...");
                $("img").attr("src", "/images/Dead.jpg");
            }
            if(full >= 100 && happ >= 100 && nrg >= 100)
            {
                $("button[type='button']").hide();
                $("button[type='submit']").show();
                $("#message").html("You win! Hooray!");
                $("img").attr("src", "/images/Love.jpg");
            }
        });
        return false;
    });
    $("button[type='submit']").click(function(){
        $.get("/reset", function(res)
        {
           initialize(); 
        });
    });
});

function initialize()
{
    $.get("/fetchdata", function(res)
    {
        $("#fullness").html(res.fullness);
        $("#happiness").html(res.happiness);
        $("#meals").html(res.meals);
        $("#energy").html(res.energy);
    });
    $("button[type='button']").show();
    $("button[type='submit']").hide();
    $("#message").html("Welcome to Dojodachi!");
    $("img").attr("src", "/images/Normal.jpg");
}