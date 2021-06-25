function AddLike(PostId, UserId) {
    console.log("/" + PostId + "/AddLike?userid=" + UserId);
    $.post(
        "/" + PostId + "/AddLike?userid=" + UserId,
        function done(data) {
            console.log($.parseJSON(data));
            $(".AddLike").css("display", "none");
            $(".RemoveLike").css("display", "inline");
            $(".LikeCounter").text("Likes: " + $.parseJSON(data)["Likes"]);
        }
    )
}

function RemoveLike(PostId, UserId) {
    console.log("/" + PostId + "/RemoveLike?userid=" + UserId);
    $.post(
        "/" + PostId + "/RemoveLike?userid=" + UserId,
        function done(data) {
            console.log($.parseJSON(data));
            $(".AddLike").css("display", "inline");
            $(".RemoveLike").css("display", "none");
            $(".LikeCounter").text("Likes: " + $.parseJSON(data)["Likes"]);
        }
    )
}