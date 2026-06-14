// Site-wide vanilla JS.
// (Interop is planned to move to TypeScript later; kept minimal for now.)
(function () {
    function currentTheme() {
        return document.documentElement.getAttribute('data-bs-theme') || 'dark';
    }

    function savedTheme() {
        try {
            return localStorage.getItem('theme') || 'dark';
        } catch (e) {
            return 'dark';
        }
    }

    function applyTheme(theme) {
        document.documentElement.setAttribute('data-bs-theme', theme);
        try {
            localStorage.setItem('theme', theme);
        } catch (e) {
            /* localStorage unavailable — ignore */
        }
    }

    function reapplySavedTheme() {
        document.documentElement.setAttribute('data-bs-theme', savedTheme());
    }

    // Add a subtle border/shadow to the sticky navbar once the page is scrolled.
    function updateNavShadow() {
        var nav = document.querySelector('.ph-nav');
        if (nav) {
            nav.classList.toggle('ph-scrolled', window.scrollY > 4);
        }
    }
    window.addEventListener('scroll', updateNavShadow, { passive: true });

    // Event delegation survives Blazor's enhanced-navigation DOM updates.
    document.addEventListener('click', function (e) {
        var toggle = e.target.closest('[data-theme-toggle]');
        if (toggle) {
            e.preventDefault();
            applyTheme(currentTheme() === 'dark' ? 'light' : 'dark');
            return;
        }

        // Close the mobile menu after tapping a nav link.
        if (e.target.closest('.ph-nav-links a')) {
            var cb = document.getElementById('ph-nav-toggle');
            if (cb) {
                cb.checked = false;
            }
        }
    });

    // Blazor enhanced navigation (SPA-like) can reset attributes on <html>,
    // dropping the chosen theme. Re-apply the saved theme after each enhanced load.
    function hookEnhancedLoad() {
        if (window.Blazor && typeof window.Blazor.addEventListener === 'function') {
            window.Blazor.addEventListener('enhancedload', function () {
                reapplySavedTheme();
                updateNavShadow();
            });
        } else {
            // Blazor runtime not ready yet — retry shortly.
            setTimeout(hookEnhancedLoad, 50);
        }
    }
    hookEnhancedLoad();
    updateNavShadow();
})();
