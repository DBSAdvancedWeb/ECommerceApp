$(document).ready(function() {

   $('.cart-btn').on('click', function(e) {
        e.preventDefault();
        let product = $(this).data('product');

        console.log(product.name);

        $.ajax({
            type: "POST",
            url: "/shoppingcart",
            data: JSON.stringify(product),
            dataType: 'json',
            contentType: "application/json",
            success: function(response) {
                // Handle success response
                console.log("Data successfully sent to /shoppingcart:", response);
            },
            error: function(xhr, status, error) {
                // Handle error response
                console.error("Error:", error);
            }
        });
   });

});