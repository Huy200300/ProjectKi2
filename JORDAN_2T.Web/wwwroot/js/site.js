var formatMoney = '{{amount}}₫';
var check_variant = true,
    check_variant_quickview = true,
    swatch_size = 0;
window.file_url = "//file.hstatic.net/200000384421/file/";
window.asset_url = "//theme.hstatic.net/200000384421/1000955298/14/?v=20";
localStorage.setItem('shop_id', '200000384421/1000955298');
window.shop = {
    terms: "",
    template: 'index',
    customer: true,
    shopCurrency: "VND",
    formatMoney: '{{amount}}₫',
    plugin: ["//theme.hstatic.net/200000384421/1000955298/14/plugin-script.js?v=20", "//theme.hstatic.net/200000384421/1000955298/14/jquery.cookie.js?v=20"],
    recaptchajs: 'https://www.google.com/recaptcha/api.js?render=6LdD18MUAAAAAHqKl3Avv8W-tREL6LangePxQLM-',
    getScriptCcd: (url, callback) => {
        $.ajax({
            type: "GET",
            url: url,
            success: callback,
            dataType: "script",
            cache: true
        });
    },
    getdatasite: (url, callback, callbackcustom) => {
        $.ajax({
            type: "GET",
            url: url,
            success: function(data) {
                callback.html(data);
                if (typeof(callbackcustom) == 'function') callbackcustom()
            },
            cache: true
        });
    },
    stylerender: ["//theme.hstatic.net/200000384421/1000955298/14/render-style.css?v=20", "//theme.hstatic.net/200000384421/1000955298/14/pe-icon-7-strokes.scss.css?v=20"],
    productjson: {
        "error": "json not allowed for this object"
    },
    recaptchacallback: () => {
        setTimeout(() => {
            $("input[name='g-recaptcha-response']").each(function() {
                let $this = $(this);
                grecaptcha.ready(function() {
                    grecaptcha.execute('6LdD18MUAAAAAHqKl3Avv8W-tREL6LangePxQLM-', {
                        action: 'submit'
                    }).then(function(token) {
                        $this.val(token);
                    });
                });
            });
        }, 3000)
    }
}
if (navigator[_0x2c0xa[2]][_0x2c0xa[1]](_0x2c0xa[0]) == -1) {
    for (let i = 0; i < window.shop.stylerender.length; i++) {
        const css = document.createElement("link");
        css.setAttribute("type", "text/css");
        css.setAttribute("rel", "stylesheet");
        css.setAttribute("href", window.shop.stylerender[i]);
        document.getElementsByTagName("head")[0].appendChild(css);
    }
    for (let i = 0; i < window.shop.plugin.length; i++) {
        const plugin = document.createElement("script");
        plugin.setAttribute("type", "text/javascript");
        plugin.setAttribute("src", window.shop.plugin[i]);
        document.getElementsByTagName("head")[0].appendChild(plugin);
    }
    $(window).load(() => {
        window.shop.getScriptCcd(window.shop.recaptchajs, window.shop.recaptchacallback());
    })
}

function fillSubCategories(countryList, subCategoryId) {
    var list = $("#" + subCategoryId);
    list.empty();

    var selectedCategory = countryList.options[countryList.selectedIndex].value;
    if (selectedCategory != null && selectedCategory != '') {
        $.getJSON("/Admin/Movies/GetSubCategoriesByCategory", { categoryId: selectedCategory }, function(subCategories) {
            if (subCategories != null && !jQuery.isEmptyObject(subCategories)) {
                $.each(subCategories, function(index, subCategory) {
                    list.append($('<option/>', {
                        value: subCategory.value,
                        text: subCategory.text
                    }));
                });
            }
        });
    }
    return;
}

function ShowSearch() {
    $('.header-search').addClass('Show');

}

function searchclose() {
    $('.header-search').removeClass('Show');

}