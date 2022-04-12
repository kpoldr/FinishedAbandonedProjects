class Apartment:

    def __init__(self, nr, size, owner, people, w_old, w_new, pay_debt, fine, received, month, pdf=True):
        self.nr = nr
        self.size = size
        self.owner = owner
        self.people = people
        self.water_old = w_old
        self.water_new = w_new
        self.pay_debt = pay_debt
        self.fine = fine
        self.received = received
        self.month = month
        self.pdf = pdf

    def update_apartment_info(self, to_update):
        for key in to_update:
            gen_key = key[0:-1]
            if gen_key == 'APARNUMBER':
                self.nr = to_update[key]
            elif gen_key == 'SIZE':
                self.size = to_update[key]
            elif gen_key == 'OWNER':
                self.owner = to_update[key]
            elif gen_key == 'PEOPLE':
                self.people = to_update[key]

    def return_info(self):
        return [self.nr, self.size, self.owner, self.people, self.water_old, self.water_new, self.pay_debt, self.fine,
                self.received, self.pdf]

    def cal_garbage_pay(self, max_aparts):
        payment = self.people * self.month.garbage

        if self.month.double_m is True:
            payment *= 2

        return payment

    def update_water_old(self, water_value):
        self.water_old = water_value

    def cal_water_dif(self):
        return self.water_new - self.water_old

    def cal_water_pay(self):
        return (self.water_new - self.water_old) * self.month.water

    def cal_con_pay(self):
        return self.size * self.month.con_pay

    def cal_next_pay_debt(self):

        return self.cal_garbage_pay() + self.cal_con_pay() + self.cal_water_pay() + self.pay_debt +\
               + self.month.electricity_p + self.fine - self.received

    def cal_pay(self):
        return self.cal_water_pay() + self.cal_con_pay() + self.cal_garbage_pay() + self.month.cal_elec_pay()

    def cal_final_pay(self):

        pay = self.cal_garbage_pay() + self.cal_con_pay() + self.cal_water_pay() + -1 * self.pay_debt \
              + self.month.electricity_p

        if pay < 0:
            return 0

        else:
            return pay

    def __repr__(self):
        return self.nr

    def __str__(self):
        return self.nr
