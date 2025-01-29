// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showToast(message, type = "error") {
    const toast = document.createElement("div");
    toast.className = `toast ${type}`;
    toast.innerHTML = `
                <span>${message}</span>
                <button class="close-btn" onclick="this.parentElement.remove()">×</button>
            `;

    // Add toast to container
    const container = document.getElementById("toast-container");
    container.appendChild(toast);

    // Automatically remove toast after 5 seconds
    setTimeout(() => {
        if (toast) {
            toast.remove();
        }
    }, 5000);
}

document.querySelectorAll('.faq-question').forEach(button => {
    button.addEventListener('click', () => {
        const faqItem = button.parentElement;

        // Toggle active class
        faqItem.classList.toggle('activeq');

        // Close other open items
        document.querySelectorAll('.faq-item').forEach(item => {
            if (item !== faqItem) {
                item.classList.remove('activeq');
            }
        });
    });
});
