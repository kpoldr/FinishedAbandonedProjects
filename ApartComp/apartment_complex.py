class ApartmentComplex:

    def __init__(self, name, size, street, town, municipality, county, postal_code, creator, phone, email, bank_name,
                 bank_nr, start_nr=1):
        self.name = name
        self.size = size
        self.street = street
        self.town = town
        self.municipality = municipality
        self.county = county
        self.postal_code = postal_code
        self.creator = creator
        self.phone = phone
        self.email = email
        self.bank_name = bank_name
        self.bank_nr = bank_nr
        self.start_nr = start_nr

    def return_info(self):
        return [self.name, self.street, self.town, self.municipality, self.county, self.postal_code, self.creator,
                self.phone, self.email, self.bank_name, self.bank_nr, self.start_nr]
