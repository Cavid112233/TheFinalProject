
function handleDashboardTabClick() {
    let dashboardTabLi = document.querySelectorAll('.tab-li');
    let dashboardContent = document.querySelectorAll('.my-account-content');

    dashboardTabLi.forEach((item, index) => {
        item.addEventListener('click', () => {
            dashboardTabLi.forEach(item => {
                item.classList.remove('tab-li-active');
            });
            item.classList.add('tab-li-active');
            dashboardContent.forEach(item => {
                item.classList.remove("my-account-content-active");
            });
            dashboardContent[index].classList.add('my-account-content-active');
        });
    });
}

document.addEventListener('click', handleDashboardTabClick);


 

function ChangeImage() {
    let mainImage = document.getElementById('MainImg');
    let detailImages = document.querySelectorAll('.detail-image')
    detailImages.forEach(image => {
        image.onclick = () => {
            mainImage.src = image.src;
        };
    });
}
document.addEventListener('click', ChangeImage);


  $(".product-main-image")
  .on("mouseover", function() {
    $(this)
      .children("#MainImg")
      .css({ transform: "scale(" + $(this).attr("data-scale") + ")" });
  })
  .on("mouseout", function() {
    $(this)
      .children("#MainImg")
      .css({ transform: "scale(1)" });
  })
  .on("mousemove", function(e) {
    $(this)
      .children("#MainImg")
      .css({
        "transform-origin":
          ((e.pageX - $(this).offset().left) / $(this).width()) * 100 +
          "% " +
          ((e.pageY - $(this).offset().top) / $(this).height()) * 100 +
          "%"
      });
  });

function handleProductTabClick() {
    let productTabLi = document.querySelectorAll('.product-tab-li');
    let productTabContent = document.querySelectorAll('.product-tab-content');

    productTabLi.forEach((item, index) => {
        item.addEventListener('click', () => {
            productTabLi.forEach(item => {
                item.classList.remove('product-tab-li-active');
            });
            item.classList.add('product-tab-li-active');
            productTabContent.forEach(item => {
                item.classList.remove("product-tab-content-active");
            });
            productTabContent[index].classList.add('product-tab-content-active');
        });
    });
}

handleProductTabClick();


function handleLoginTabClick() {
    let loginTabLi = document.querySelectorAll('.log-reg-tab-li');
    let loginTabContent = document.querySelectorAll('.login-register-tab-content');

    loginTabLi.forEach((item, index) => {
        item.addEventListener('click', () => {
            loginTabLi.forEach(item => {
                item.classList.remove('log-reg-tab-li-active');
            });
            item.classList.add('log-reg-tab-li-active');
            loginTabContent.forEach(item => {
                item.classList.remove('login-register-tab-content-active');
            });
            loginTabContent[index].classList.add('login-register-tab-content-active');
        });
    });
}

handleLoginTabClick();


function handleMiniCart() {
    const miniCart = document.querySelector('#MiniCart');
    const miniCartBtn = document.querySelector('.minicart-btn');
    const miniCartCloseBtn = document.querySelector('.minicart-close-btn');

    miniCartBtn.addEventListener('click', () => {
        miniCart.classList.add('active');
    });

    miniCartCloseBtn.addEventListener('click', () => {
        miniCart.classList.remove('active');
    });
}

handleMiniCart();

$(document).on('click', '.plus-qty', function () {
    let inputValueAll = document.querySelectorAll('.product-cart-count');
    let productId = $(this).attr('data-id');
    let index = Array.from(document.querySelectorAll('.plus-qty')).indexOf(this);

    let inputValue = Number(inputValueAll[index].value);
    inputValueAll[index].value = inputValue + 1;

    fetch('product/ChangeBasketProductCount?id=' + productId + '&count=' + inputValueAll[index].value)
        .then(res => res.text())
        .then(data => {
            $('.productTable').html(data);
            fetch('product/ChangeCartProductCount?id=' + productId + '&count=' + inputValueAll[index].value)
                .then(res1 => res1.text())
                .then(data1 => {
                    $('.cartbody').html(data1);
                    fetch('product/RefreshCartTotal')
                        .then(res2 => res2.text())
                        .then(data2 => {
                            $('.cart-total-box').html(data2);
                        })
                })
        });
        
    
});

$(document).on('click', '.minus-qty', function () {
    let inputValueAll = document.querySelectorAll('.product-cart-count');
    let productId = $(this).attr('data-id');
    let index = Array.from(document.querySelectorAll('.minus-qty')).indexOf(this);

    let inputValue = Number(inputValueAll[index].value);
    if (inputValue > 1) {
        inputValueAll[index].value = inputValue - 1;

        fetch('product/ChangeBasketProductCount?id=' + productId + '&count=' + inputValueAll[index].value)
            .then(res => res.text())
            .then(data => {
                $('.productTable').html(data);
                fetch('product/ChangeCartProductCount?id=' + productId + '&count=' + inputValueAll[index].value)
                    .then(res => res.text())
                    .then(data => {
                        $('.cartbody').html(data);
                        fetch('product/RefreshCartTotal')
                            .then(res2 => res2.text())
                            .then(data2 => {
                                $('.cart-total-box').html(data2);
                            })
                    })
            });
    }
});




////////////
$(document).on('click', '.categoryFilter', function () {
    let categoryId = $(this).attr('data-categoryId');
    let pageIndex = $(".pageIn").attr('data-pageIndex');
    let materialId = $(this).attr('data-materialId');
    fetch("/shop/shopfilters?categoryId=" + categoryId + "&materialId=" + materialId + "&pageIndex=" + pageIndex )
        .then(res => {
            return res.text();
        })
        .then(data => {
        
            $('#Shop').html(data);
        });
});

$(document).on('click', '.pageIn', function () {
    let categoryId = $(this).attr('data-categoryId');
    let pageIndex = $(this).attr('data-pageIndex');
    let materialId = $(this).attr('data-materialId');
    fetch("/shop/shopfilters?categoryId=" + categoryId + "&materialId=" + materialId + "&pageIndex=" + pageIndex)
        .then(res => {
            return res.text();
        })
        .then(data => {
            $('#Shop').html(data);
        });
});

$(document).on('click', '.materialSelector', function () {
    let categoryId = $(this).attr('data-categoryId');
    let pageIndex = $(".pageIn").attr('data-pageIndex');
    let materialId = $(this).attr('data-materialId');
    fetch("/shop/shopfilters?categoryId=" + categoryId + "&materialId=" + materialId + "&pageIndex=" + pageIndex)
        .then(res => {
            return res.text();
        })
        .then(data => {

            $('#Shop').html(data);
        });
});

$(document).on('click', '#filterBtn', function () {
    let categoryId = $(this).attr('data-categoryId');
    let pageIndex = $(".pageIn").attr('data-pageIndex');
    let materialId = $(this).attr('data-materialId');
    let minPrice = $("#min-value").text();
    let maxPrice = $("#max-value").text();
    console.log(minPrice);
    console.log(maxPrice);
    fetch("/shop/shopfilters?categoryId=" + categoryId + "&materialId=" + materialId + "&minPrice=" + minPrice + "&maxPrice=" + maxPrice + "&pageIndex=" + pageIndex)
        .then(res => {
            return res.text();
        })
        .then(data => {

            $('#Shop').html(data);
        });
});

//Add To Cart
$(".minicart-badge").html($(".cartcount").text());

$(document).on("click", ".addToCart", function (e) {
    e.preventDefault();
    let productId = $(this).data('id');
    fetch("basket/AddToBasket?id=" + productId)
        .then(res => {
            return res.text();
        })
        .then(data => {
            $(".cartbody").html(data);
            $(".minicart-badge").html($(".cartcount").text());
        });
});


//Delete from Basket
console.log(window.location.pathname)
$(document).on('click', ".remove_btn", function (e) {
    e.preventDefault();
    let url = $(this).attr('href');
    fetch(url)
        .then(res => {
            return res.text();
        })
        .then(data => {
            $(".cartbody").html(data);
            $(".minicart-badge").html($(".cartcount").text())
            fetch("basket/RefreshIndex")
                .then(res1 => {
                    return res1.text();
                })
                .then(data1 => {
                    $(".productTable").html(data1);
                    $(".minicart-badge").html($(".cartcount").text())
                    fetch('product/RefreshCartTotal')
                        .then(res2 => res2.text())
                        .then(data2 => {
                            $('.cart-total-box').html(data2);
                        })
                })

        })

});

//Delete from Cart
$(document).on('click', ".cartdelete", function (e) {
    e.preventDefault();

    let url = $(this).attr('href');
    fetch(url).then(res => res.text()).then(data => {
        $(".productTable").html(data);
        $(".minicart-badge").html($(".cartcount").text())
        fetch('basket/RefreshBasket').then(res1 => res1.text())
            .then(data1 => {
                $(".cartbody").html(data1);
                $(".minicart-badge").html($(".cartcount").text())
                fetch('product/RefreshCartTotal')
                    .then(res2 => res2.text())
                    .then(data2 => {
                        $('.cart-total-box').html(data2);
                    })
            })
    })

})
//Add Address
$(document).on('click', '.addAddress', function (e) {
    e.preventDefault();
    $('.user-addresses').addClass('d-none');
    $('.addressForm').removeClass('d-none');
    $('.addAddress').addClass('d-none');

})
//Toogle table
$(document).on('click', '.accordion-toggle', function () {
    var targetRow = $(this).closest('tr').next('.hiddenRow');
    targetRow.toggleClass('show');
});

//$('.accordian-body').on('show.bs.collapse', function () {
//    $(this).closest("table")
//        .find(".collapse.in")
//        .not(this)
//    //.collapse('toggle')
//})

