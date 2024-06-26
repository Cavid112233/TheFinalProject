$(document).ready(function () {
    $(document).on('click', '.deleteImage', function (e) {
        e.preventDefault();
        let url = $('.deleteImage').attr('href');
        let imageId = $('.deleteImage').attr('data-imageId');
        fetch(url + "?imageId=" + imageId)
            .then(res => res.text())
            .then(data => {
                $('.productImages').html(data)
            })
    })
    $(".show-more").click(function (event) {
        event.preventDefault();
        
        $(this).closest(".product-description-short").hide();
        $(this).closest(".product-description-short").next(".product-description-full").show();
    });
    $(".show-less").click(function (event) {
        event.preventDefault();
        $(this).closest(".product-description-full").hide();
        $(this).closest(".product-description-full").prev(".product-description-short").show();
    });

    

})
const menuItems = document.querySelectorAll('.menu-item');

menuItems.forEach(item => {
    item.addEventListener('click',() => {
        
        menuItems.forEach(item => {
            item.classList.remove('active');

        });
        event.currentTarget.classList.add('active');
    });
});