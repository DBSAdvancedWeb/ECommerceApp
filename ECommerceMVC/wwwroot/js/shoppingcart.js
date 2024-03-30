$(document).ready(function() {

   $('.cart-btn').on('click', function(e) {
        e.preventDefault();
        let product = $(this).data('product');
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

   $('.quantity').on('change', function(e) {
        
        let $this = $(e.target);    
        let quantity = $($this).val();
        let unitPrice = parseFloat($($this).parent().parent().find('.unit-price').text());
        let unitByQuantity = $($this).parent().parent().find('.unit-quantity');
        
        let total = unitPrice * quantity;
        let formattedPrice = total.toFixed(2);
        unitByQuantity.text(formattedPrice);
        
        //calculate total
        let totalPrice = $('#totalpurchase');
        let totalCost = 0;
    
        $('.unit-quantity').each(function() {
            totalCost += parseFloat($(this).text());
        });
        
        totalPrice.text(totalCost.toFixed(2));
   });

});