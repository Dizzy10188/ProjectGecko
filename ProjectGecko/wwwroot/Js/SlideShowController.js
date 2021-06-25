var postList = document.getElementsByClassName("postimages");
var pageNumberHolder = 0;

for (var i = 1; i < postList.length; i++) {
    var curPost = postList.item(i);
    console.log(curPost.style);
    curPost.style.display = "none";
}

function plusSlides(indexer) {
    if (indexer < 0 && pageNumberHolder > 0) {
        console.log(indexer);
        console.log(pageNumberHolder);
        pageNumberHolder += indexer;
        for (var i = 0; i < postList.length; i++) {
            var curPost = postList.item(i);
            curPost.style.display = "none";
        }
        postList.item(pageNumberHolder).style.display = "inline";
    }

    if (indexer > 0 && pageNumberHolder < postList.length - 1) {
        console.log(indexer);
        console.log(pageNumberHolder);
        pageNumberHolder += indexer;
        for (var i = 0; i < postList.length; i++) {
            var curPost = postList.item(i);
            curPost.style.display = "none";
        }
        postList.item(pageNumberHolder).style.display = "inline";
    }
}

