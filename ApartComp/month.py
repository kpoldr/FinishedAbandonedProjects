class Month:

    def __init__(self, name, date, con_pay, garbage, water, electricity_p, double_m=False):
        self.name = name
        self.date = date
        self.con_pay = con_pay
        self.garbage = garbage
        self.water = water
        self.electricity_p = electricity_p
        self.double_m = double_m

    def return_info(self):
        return [self.name, self.date, self.con_pay, self.garbage, self.water, self.electricity_p, self.double_m]

    def cal_elec_pay(self):
        return self.electricity_p

    def update_month_info(self, to_update):
        for key in to_update:
            if key == 'CONPAY':
                self.con_pay = to_update[key]
            if key == 'GARBAGEPAY':
                self.garbage = to_update[key]
            if key == 'WATERPAY':
                self.water = to_update[key]
            if key == 'ELECTRICITYPAY':
                self.electricity_p = to_update[key]

    def cal_garbage_person(self, max_aparts):
        return


