$(document).ready(function () {
 
    $('#itemSelect').select2({
        ajax: {
            url: '/customer/home/getitem',
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    q: params.term // Search term entered by user
                };
            },
            processResults: function (data) {
                return {

                    results: $.map(data.data, function (obj) {
                        console.log(obj)
                       tick(obj)

                        return { id: obj.id, text: obj.name };
                    })
                };
            },
            
            cache: true
        },
        placeholder: 'Select an item',
        minimumInputLength: 1
    });
   
    $(document).ready(function () {
        $("#datepicker").datepicker({
            dateFormat: "dd/mm/yy"
        });
    });

    //$(document).ready(function () {
    //    $('#timepicker').timepicker();
    //});


    //$(document).ready(function () {
    //    $(".opening-time, .closing-time").datepicker({
    //        timeFormat: "hh:mm tt",
    //        stepMinute: 15, // Optional: Set time interval
    //        controlType: "select",
    //    });
    //});



   
           


    function tick(obj) {
        $('input[type="checkbox"]').prop('checked', false)
        var langs = obj.languages.split(", ")
        langs.forEach(l => {
            $('#' + l.toLowerCase()).prop('checked', true)

        })
        $('#datetime-picker').val(obj.date)
        $('#opening-time').val(obj.openTime)
        $('#closing-time').val(obj.closeTime)


        console.log(langs)
    }



  
      
 

 $('#submit').click(function(event) {
        // Get selected languages from checkboxes
        var selectedLanguages = $('input[name="languages"]:checked').map(function(){
            return $(this).val();
        }).get();

     var selectedDate = $('#datetime-picker').val(); // Get the selected date from the input field
   //  var selectedTime = $('#timepicker').val(); // Get the selected time from the input field
     var openingTime = $("#opening-time").val();
     var closingTime = $("#closing-time").val();



     var country = {
         "id": $('#itemSelect').select2('data')[0].id,
         "name": $('#itemSelect').select2('data')[0].text,   
         "Date": selectedDate, // Add the selected date to the country object
         "OpenTime": openingTime,
         "CloseTime": closingTime,
         //"Time": selectedTime,
     }
     console.log(country)
   

    

        if (selectedLanguages.length > 0) {
            $('#itemSelect').append(new Option(selectedLanguages.join(', '), selectedLanguages));
            $('#itemSelect').trigger('change'); // Notify Select2 of the change
     }
     var req = { "countryObj": country, "languages": selectedLanguages }
     $.ajax({
         type: 'POST',
         url: '/customer/home/country',
         dataType: 'json',
         delay: 250,
         data: req,
         processResults: function (data) {
              console.log(data)

             
         }
     });

     event.preventDefault();
    });
});