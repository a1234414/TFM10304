// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var slideIndex = 1;
showSlides(slideIndex);

function plusSlides(n) {
    showSlides(slideIndex += n);
}

function currentSlide(n) {
    showSlides(slideIndex = n);
}

function showSlides(n) {
    var i;
    var slides = document.getElementsByClassName("mySlides");
    var dots = document.getElementsByClassName("dot");
    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }
    slides[slideIndex - 1].style.display = "block";
    dots[slideIndex - 1].className += " active";
}


function update() {
    document.getElementById("ProdictDetail").getBoundingClientRect();
}

document.addEventListener('scroll', update);
update();

var positionData = document.getElementById("ProdictDetail").getBoundingClientRect();
var xPosition = positionData.left + window.scrollX;
var yPosition = positionData.top + window.scrollY;
var elementWidth = positionData.width;
var elementHeight = positionData.height;


//固定方法
window.onscroll = function () { fixFunction(), fix2Function() };

document.getElementById("listA").style.display = "affix";
