# 🧾 Modul: Kunden

## ✅ Fertiggestellt
- [x] Kunden anlegen über Dialog
- [x] Eingabeformular mit Validierung
- [x] Kunden speichern in SQLite-Datenbank
- [x] Kunden anzeigen in DataGrid
- [x] Kunden bearbeiten (mit eingeschränkten Feldern bei vorhandenen Vorgängen)
- [x] Löschvermerk statt Löschen (IsActive = false)
- [x] Gefilterte Kundenliste (nur aktive Kunden)
- [x] Reaktivierung vorbereitbar über Adminrolle
- [x] Speicherung der Kundendaten funktioniert dauerhaft
- [x] Bearbeitungsdialog gefüllt mit Kundendaten
- [x] Nur einmaliges EF Tracking (UpdateAsync repariert)

---

## 🛠 Noch offen (Phase 2+)
- [ ] DSGVO-Einwilligung (`HasDsgvoConsent`) ergänzen
- [ ] SEPA-Feld (`HasSepaAuthorization`) ergänzen
- [ ] IBAN, BIC, Bankname hinzufügen (mit Validierung)
- [ ] Auto-generierte Kundennummer (`K20250001` etc.)
- [ ] Änderungsverlauf: Wer hat wann gespeichert?
- [ ] Mehrere Ansprechpartner je Kunde (optional)
- [ ] Suche & Filter in der Kundenliste (Name, Ort, aktiv/inaktiv)
- [ ] Exportmöglichkeit (CSV / PDF / DSGVO-Auszug)
- [ ] Bearbeiten-Dialog visuell trennen von Neuanlage (Titelzeile, Icon etc.)

---

## 💡 Ideen / Nice-To-Have
- [ ] Kunden aus CSV importieren
- [ ] Kontakte nach Typ klassifizieren (Rechnung, Technik, Support)
- [ ] Kundenfarben oder Tags (z. B. „VIP“, „intern“, „Testkunde“)
- [ ] Benachrichtigung bei Inaktivität > 12 Monate
