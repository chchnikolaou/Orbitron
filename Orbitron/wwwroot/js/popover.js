//document.addEventListener('DOMContentLoaded', function () {

//    const trigger = document.querySelector('.popover-btn');
//    const container = document.querySelector('.popover-container');

//        trigger.addEventListener('click', () => {
//        // Toggle the 'active' class to show/hide the popover
//        container.classList.toggle('active');
//        });

//        // Optional: Close the popover when clicking outside
//        document.addEventListener('click', (e) => {
//            if (!container.contains(e.target)) {
//        container.classList.remove('active');
//            }
//        });
//    });

document.addEventListener('DOMContentLoaded', function () {
    const trigger = document.querySelector('.popover-btn');
    const container = document.querySelector('.popover-container');

    trigger.addEventListener('mouseover', () => {
        container.classList.add('popover-active');
    });

    trigger.addEventListener('mouseout', () => {
        container.classList.remove('popover-active');
    });
});
