$(document).ready(function () {
    $('#itemSelect').select2({
        ajax: {
            url: '/customer/home/getall',
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    q: params.term 
                };
            },
            processResults: function (data) {
                console.log(data)
                data.data.forEach(product => {
                    renderbook(product)
                })
                return {
                   
                    results: $.map(data.data, function (obj) {
                        console.log(obj)
                        return { id: obj.id, text: obj.title };
                    })
                };
            },
            cache: true
        },
        placeholder: 'Select an item',
        minimumInputLength: 1
    });
});


function renderbook(product) {
    var html = `
        <div class="col-lg-3 col-sm-6">
        <div class="row p-2">
            <div class="col-12 p-1">
                <div class="card border-0 p-3 shadow border-top border-5 rounded">
                   
                    <img src="${product.productImages[0].imageUrl}" class="card-img-top rounded">
                     

                        <div class="card-body pb-0">
                            <div class="pl-1">
                                <p class="card-title h5 text-dark opacity-75 text-uppercase text-center">${product.title}</p>
                                <p class="card-title  text-warning text-center">by<b>${product.author}</b></p>
                            </div>
                            <div class="pl-1">
                                <p class=" text-dark text-opacity-75  text-center mb-0">
                                    List Price:
                                    <span class="text-decoration-line-through">
                                       ₹ ${product.listPrice}
                                    </span>
                                </p>

                            </div>
                            <div class="pl-1">
                                <p class=" text-dark text-opacity-75  text-center">
                                    As low as:
                                    <span>
                                       ₹ ${product.price100}
                                    </span>
                                </p>

                            </div>
                        </div>

                         <div>
                         
                        <a href="/Customer/Home/Details?productId=${product.id}" asp-action="Details"
                        asp-route-productId="${product.id}"
                        class="btn btn-primary bg-gradient border-0 form-control">
                            Details
                        </a>
                    </div>
                </div>
            </div>
            </div >
                </div >
`
       
        
    $(".products").empty()
    $(".products").append(html)

}