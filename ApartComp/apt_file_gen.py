import pickle
from apartment import Apartment

# months = [january, february, march, april, may, june, july, august, september, october, november, december]


class AptFileGen:

    def __init__(self, name, months):
        self.name = name
        self.months = months

    def generate_apartment_file(self, file):
        pickle.dump(self.months, open(f'{file}.uh', 'wb'))

    def load_apartment_file(self, file):
        return pickle.load(open(f'{file}.uh', "rb"))
