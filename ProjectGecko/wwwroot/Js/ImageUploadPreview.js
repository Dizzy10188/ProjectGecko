var imgPreview = document.getElementById("ImagePreviewer");
var imgUploader = document.getElementById("ImageUploader");

imgUploader.addEventListener("change", event => {
    console.log(event);
    console.log(imgUploader.files)
    if (imgUploader.files && imgUploader.files[0]) {
        console.log(URL.createObjectURL(imgUploader.files[0]));
        imgPreview.src = URL.createObjectURL(imgUploader.files[0]); // set src to blob url
    }
});