// Write your JavaScript code.


$(window).on('load', function () {
    $('.add-grade-form').submit(function (event){
        event.preventDefault();

        var thisForm = $(this);

        var data = thisForm.serialize();

        let url = thisForm.attr("action");

        let posting = $.post(url, data);

        posting.done(function (data) {
            if (data.status == 'true') {
                let studentId = data.studentId;
                thisForm.find('select[name="StudentId"]').find('option[value=' + studentId + ']').remove();
                if (thisForm.find('select[name="StudentId"]').has('option').length == 0) {
                    toastr.success('All sutdents are graded', 'Server message:');

                }
            }
            else {
                toastr.error('Something went wrong. Please refresh the page and try again.', 'Server message:');
            }
           
            console.log(data);
            
    });
    });
});