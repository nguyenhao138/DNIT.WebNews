/*
Template Name: Abstack - Responsive Admin & Dashboard Template
Author: CoderThemes
File: Datatables init js
*/


    // Default Datatable
    $(document).ready(function() {
        $('#datatable').DataTable();

        //Buttons examples
        var table = $('#datatable-buttons').DataTable({
            lengthChange: false,
            buttons: ['copy', 'excel', 'pdf']
        });

        table.buttons().container()
                .appendTo('#datatable-buttons_wrapper .col-md-6:eq(0)');
    } );
