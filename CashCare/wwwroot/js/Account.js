function checkPhoneFormat() {
    var phoneNumberInput = document.getElementById('floatingInputGridDescription');
    var phoneNumber = phoneNumberInput.value;
    var warningLabel = document.getElementById('warning');

    // Regex pour l'Allemagne et la France
    var regexGermany = /^\+49\d{10,12}$/; // Exemple: +4917623709481
    var regexFrance = /^\+33\d{9}$/; // Exemple: +33612345678

    if (regexGermany.test(phoneNumber) || regexFrance.test(phoneNumber)) {
        warningLabel.style.display = 'none'; // Masquer le label en cas de numéro valide
        return true; // Numéro valide
    } else {
        warningLabel.style.display = 'block'; // Afficher le label en cas de numéro invalide
        return false; // Numéro invalide
    }
}