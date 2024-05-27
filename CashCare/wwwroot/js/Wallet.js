function validateDecimalInput(input) {
    // Remove non-numeric characters except decimal point
    input.value = input.value.replace(/[^0-9.]/g, '');
    // Ensure there's only one decimal point
    const parts = input.value.split('.');
    if (parts.length > 2) {
        input.value = parts[0] + '.' + parts.slice(1).join('');
    }
}