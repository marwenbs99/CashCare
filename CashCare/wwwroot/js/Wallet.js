function validateDecimalInput(input) {
    
    var selectedLanguage = localStorage.getItem('selectedLanguage');

    if (selectedLanguage === 'fr' || selectedLanguage === 'de') {
        input.value = input.value.replace(/[^0-9,]/g, '');
        // Ensure there's only one decimal point
        const parts = input.value.split(',');
        if (parts.length > 2) {
            input.value = parts[0] + ',' + parts.slice(1).join('');
        }
    } else
    {
        input.value = input.value.replace(/[^0-9.]/g, '');
        // Ensure there's only one decimal point
        const parts = input.value.split('.');
        if (parts.length > 2) {
            input.value = parts[0] + '.' + parts.slice(1).join('');
        }
    }


   
}