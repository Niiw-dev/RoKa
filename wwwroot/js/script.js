const scrollWrapper = document.getElementById('scrollWrapper');

let isDown = false;
let startX;
let scrollLeft;

scrollWrapper.addEventListener('mousedown', (e) => {
    isDown = true;
    scrollWrapper.classList.add('active');
    startX = e.pageX - scrollWrapper.offsetLeft;
    scrollLeft = scrollWrapper.scrollLeft;
});

scrollWrapper.addEventListener('mouseleave', () => {
    isDown = false;
});

scrollWrapper.addEventListener('mouseup', () => {
    isDown = false;
});

scrollWrapper.addEventListener('mousemove', (e) => {
    if (!isDown) return;
    e.preventDefault();
    const x = e.pageX - scrollWrapper.offsetLeft;
    const walk = (x - startX) * 1.5; // sensibilidad
    scrollWrapper.scrollLeft = scrollLeft - walk;
});