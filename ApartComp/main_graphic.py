import PySimpleGUI as sg
import pickle
from apartment import Apartment
from month import Month
from apartment_complex import ApartmentComplex
from pdf_gen import PdfGen

headings = ['Korteri nr', 'Suurus', 'Omanik', 'In arv', 'Veenäit', 'Veenäit (uus)', 'Ettem/Võlg', 'Viivis', 'Laekunud', 'PDF']
ap_info = [('APARNUMBER', 8), ('SIZE', 6), ('OWNER', 20), ('PEOPLE', 7), ('WATEROLD', 12),
           ('WATERNEW', 12), ('PREMONEY', 9), ('FINE', 9), ('RECEIVED', 9)]
months = ['Jaanuar', 'Veebruar', 'Märts', 'Aprill', 'Mai', 'Juuni', 'Juuli', 'August', 'September', 'Oktoober',
          'November', 'Detsember']
max_aparts = 3
apartments = []
sg.theme('Default 1')

current_month = 1
months_dict = {
    1: [[], Month('Jaanuar', '', '', '', '', '', '')],
    2: [[], Month('Veebruar', '', '', '', '', '', '')],
    3: [[], Month('Märts', '', '', '', '', '', '')],
    4: [[], Month('Aprill', '', '', '', '', '', '')],
    5: [[], Month('Mai', '', '', '', '', '', '')],
    6: [[], Month('Juuni', '', '', '', '', '', '')],
    7: [[], Month('Juuli', '', '', '', '', '', '')],
    8: [[], Month('August', '', '', '', '', '', '')],
    9: [[], Month('September', '', '', '', '', '', '')],
    10: [[], Month('Oktoober', '', '', '', '', '', '')],
    11: [[], Month('November', '', '', '', '', '', '')],
    12: [[], Month('Detsember', '', '', '', '', '', '')]
}

months_to_int = {
    'Jaanuar': 1,
    'Veebruar': 2,
    'Märts': 3,
    'Aprill': 4,
    'Mai': 5,
    'Juuni': 6,
    'Juuli': 7,
    'August': 8,
    'September': 9,
    'Oktoober': 10,
    'November': 11,
    'Detsember': 12
}
apartment_complex = ApartmentComplex('', 0, '', '', '', '', '', '', '', '', '', '')


def initiate_apartments():
    for month in months_dict:
        if month == 'Apartment_complex':
            break
        month_aparts = []
        for i in range(max_aparts):
            month_aparts.append(Apartment('', '', '', '', '', '', '', '', '', '', ''))

        months_dict[month][0] = month_aparts


def defualt_disable():
    """
    Disables all usually unchangeable variables
    """

    for i in range(max_aparts):
        window.FindElement(f'APARNUMBER{i}').Update(disabled=True)
        window.FindElement(f'SIZE{i}').Update(disabled=True)


def defualt_enable():
    """
    Enables all usually unchangeable variables
    """
    for i in range(max_aparts):
        window.FindElement(f'APARNUMBER{i}').Update(disabled=False)
        window.FindElement(f'SIZE{i}').Update(disabled=False)


def check_january():
    if current_month == 1:
        disable = False
    else:
        disable = True

    for i in range(max_aparts):
        window.FindElement(f'WATEROLD{i}').Update(disabled=disable)


def update_water():
    for month in months_dict:
        for i in range(max_aparts):
            if months_dict[month][0][i].water_new != '':
                months_dict[month + 1][0][i].water_old = months_dict[month][0][i].water_new
        if month == 11:
            break


def update_month(month):
    months_dict[month][0] = []

    months_dict[month][1] = Month(month, values['DATE'], values['CONPAY'], values['GARBAGEPAY'], values['WATERPAY'],
                                  values['ELECTRICITYPAY'], values['DGARBAGE'])

    for i in range(max_aparts):
        nr = values[f'APARNUMBER{i}']
        size = values[f'SIZE{i}']
        owner = values[f'OWNER{i}']
        people = values[f'PEOPLE{i}']
        water_old = values[f'WATEROLD{i}']
        water_new = values[f'WATERNEW{i}']
        pay_debt = values[f'PREMONEY{i}']
        fine = values[f'FINE{i}']
        received = values[f'FINE{i}']
        pdf = values[f'PDF{i}']

        months_dict[current_month][0].append(
            Apartment(nr, size, owner, people, water_old, water_new, pay_debt, fine, received,
                      months_dict[current_month][1], pdf))


def clear_update():
    """
    Clears all inputs
    """
    for i in range(max_aparts):
        window.FindElement(f'APARNUMBER{i}').Update('')
        window.FindElement(f'SIZE{i}').Update('')
        window.FindElement(f'OWNER{i}').Update('')
        window.FindElement(f'PEOPLE{i}').Update('')
        window.FindElement(f'WATEROLD{i}').Update('')
        window.FindElement(f'WATERNEW{i}').Update('')
        window.FindElement(f'PREMONEY{i}').Update('')

    window.FindElement('DATE').Update('')
    window.FindElement('CONPAY').Update('')
    window.FindElement('GARBAGEPAY').Update('')
    window.FindElement('WATERPAY').Update('')
    window.FindElement('ELECTRICITYPAY').Update('')
    window.FindElement('DGARBAGE').Update('')


def read_month(month):
    """
    Reads info from the selected month.
    If the given month is empty, just clears all inputs, else fills them with saved info
    """
    cur_aparts = month[0]
    cur_month = month[1]

    for i in range(max_aparts):
        apartment = cur_aparts[i].return_info()
        window.FindElement(f'APARNUMBER{i}').Update(apartment[0])
        window.FindElement(f'SIZE{i}').Update(apartment[1])
        window.FindElement(f'OWNER{i}').Update(apartment[2])
        window.FindElement(f'PEOPLE{i}').Update(apartment[3])
        window.FindElement(f'WATEROLD{i}').Update(apartment[4])
        window.FindElement(f'WATERNEW{i}').Update(apartment[5])
        window.FindElement(f'PREMONEY{i}').Update(apartment[6])
        window.FindElement(f'FINE{i}').Update(apartment[7])
        window.FindElement(f'RECEIVED{i}').Update(apartment[6])

    window.FindElement('DATE').Update(cur_month.date)
    window.FindElement('CONPAY').Update(cur_month.con_pay)
    window.FindElement('GARBAGEPAY').Update(cur_month.garbage)
    window.FindElement('WATERPAY').Update(cur_month.water)
    window.FindElement('ELECTRICITYPAY').Update(cur_month.electricity_p)
    window.FindElement('DGARBAGE').Update(cur_month.double_m)


# def text_over_input(headings, apart_info, i):
#     """
#     Aligns text over the input. Achieved by creating the given input and text in the same column.
#     Inputs are given an unique key.
#     """
#
#     if apart_info[0] in ['APARNUMBER', 'SIZE', 'PEOPLE']:
#         return sg.Column([[sg.Text(headings[i], pad=(2, 3))],
#                           [sg.InputText(key=f'{apart_info[0]}0', size=(apart_info[1], 1), pad=(3, 0),
#                                         justification='c')]], pad=(0, 0))
#
#     elif apart_info[0] == 'RECEIVED':
#         return sg.Column([[sg.Text(f'{headings[i]}       PDF', pad=(0, 3), justification='l')],
#                           [sg.InputText(key=f'{apart_info[0]}0', size=(apart_info[1], 1), pad=(3, 0),
#                                         justification='c'), sg.Text('|'),
#                            sg.Checkbox('', pad=(0, 0), default=True,
#                                        key='PDF0')]], pad=(0, 3))
#     else:
#         return sg.Column([[sg.Text(headings[i], pad=(2, 3))],
#                           [sg.InputText(key=f'{apart_info[0]}0', size=(apart_info[1], 1), pad=(3, 0))]], pad=(0, 0))
#

def text_over_input(col):
    """
    Aligns text over the input. Achieved by creating the given input and text in the same column.
    Inputs are given an unique key.
    """


    a = []

    a.append([sg.Text(headings[col], pad=(2, 3))])

    for i in range(max_aparts):

        if col != 9:
            a.append([sg.InputText(key=f'{ap_info[col][0]}{i}', size=(ap_info[col][1], 1), pad=(3, 3))])

        else:
            a.append([sg.Checkbox('', pad=(0, 0), default=True, key=f'PDF{i}')])
    print(a)

    return sg.Column(a, pad=(0, 0))


def return_columns(apart_info):
    """
    Creates an input column for the max number of apartments. Also gives each input an unique key.
    """

    if apart_info[0] in ['APARNUMBER', 'SIZE', 'PEOPLE']:
        return sg.Column(
            [[sg.InputText(key=f'{apart_info[0]}{i}', size=(apart_info[1], 1), pad=(3, 3), justification='c')] for i in
             range(1, max_aparts)]
            , pad=(0, 0))
    elif apart_info[0] == 'RECEIVED':
        return sg.Column([[sg.InputText(key=f'{apart_info[0]}{i}', size=(apart_info[1], 1), pad=(3, 3)), sg.Text('|'),
                           sg.Checkbox('', pad=(0, 0), default=True,
                                       key=f'PDF{i}')] for i in
                          range(1, max_aparts)]
                         , pad=(0, 0))
    else:
        return sg.Column(
            [[sg.InputText(key=f'{apart_info[0]}{i}', size=(apart_info[1], 1), pad=(3, 3))] for i in
             range(1, max_aparts)]
            , pad=(0, 0))


def check_numbers():
    allowed = check_float()

    for i in range(max_aparts):
        if values[f'PDF{i}'] is True:
            for key in ['PEOPLE']:

                cur_value = values[f'{key}{i}']
                try:
                    window.FindElement(f'{key}{i}').Update(value=int(values[f'{key}{i}']), background_color='white')

                except ValueError:
                    window.FindElement(f'{key}{i}').Update(background_color='pink')
                    allowed = False

    return allowed


def check_float():
    allowed = True

    for i in range(max_aparts):
        if values[f'PDF{i}'] is True:
            for key in ['SIZE', 'PREMONEY', 'FINE', 'RECEIVED', 'WATEROLD', 'WATERNEW']:
                try:
                    cur_value = values[f'{key}{i}']

                    window.FindElement(f'{key}{i}').Update(value=float(cur_value), background_color='white')

                except ValueError:
                    window.FindElement(f'{key}{i}').Update(background_color='pink')
                    allowed = False

    for key in ['CONPAY', 'GARBAGEPAY', 'WATERPAY', 'ELECTRICITYPAY']:

        try:
            cur_value = float(values[key])
            window.FindElement(key).Update(value=cur_value, background_color='white')

        except ValueError:
            window.FindElement(key).Update(background_color='pink')
            allowed = False

    return allowed


def update_strings():
    aparts = months_dict[current_month][0]
    month = months_dict[current_month][1].return_info()
    months_dict[current_month][1] = Month(month[0], month[1], float(month[2]), float(month[3]), float(month[4]),
                                          float(month[5]), month[6])
    apartment_list = []

    for i in range(max_aparts):
        apart = aparts[i].return_info()
        print(apart)
        apartment_list.append(Apartment((apart[0]), float(apart[1]), apart[2], int(apart[3]), float(apart[4]),
                                        float(apart[5]), float(apart[6]), float(apart[7]), float(apart[8]),
                                        months_dict[current_month][1]))

    months_dict[current_month][0] = apartment_list


def update_month_apartment():
    update = False
    update_info = copy_data_window()
    apartments_info = []

    if len(update_info[0]) > 0:
        for i in range(max_aparts):
            apartment_dict = {}
            for key in update_info[0]:
                apartment_dict[f'{key}{i}'] = values[f'{key}{i}']
            apartments_info.append(apartment_dict)
        update = True

    if len(update_info[1]) > 0:
        for key in update_info[1]:
            update_info[1][key] = values[key]
        update = True
    if update:
        for month in months_dict:
            months_dict[month][1].update_month_info(update_info[1])
            for i in range(max_aparts):
                months_dict[month][0][i].update_apartment_info(apartments_info[i])


def copy_data_window():
    to_check_apar = ['APARNUMBER', 'SIZE', 'OWNER', 'PEOPLE']
    to_check_month = ['CONPAY', 'GARBAGEPAY', 'WATERPAY', 'ELECTRICITYPAY']

    to_update = [{}, {}]

    layout_win = [[[sg.Text('Mida vaja kopeerida:')]],
                  [sg.Col([[sg.Checkbox('Korteri numbrid', pad=(0, 0), default=True,
                                        key='APARNUMBER')],
                           [sg.Checkbox('Korteri suuresed', pad=(0, 0), default=True,
                                        key='SIZE')],
                           [sg.Checkbox('Korteri omanikud', pad=(0, 0), default=True,
                                        key='OWNER')],
                           [sg.Checkbox('Korteri inimste arvud', pad=(0, 0), default=True,
                                        disabled=False,
                                        key='PEOPLE')],
                           [sg.Text('-----------------------------------------------')],
                           [sg.Checkbox('Kuu osamaks', pad=(0, 0), default=True,
                                        key='CONPAY')],
                           [sg.Checkbox('Kuu prügi hind', pad=(0, 0), default=True,
                                        key='GARBAGEPAY')],
                           [sg.Checkbox('Kuu vee hind', pad=(0, 0), default=True,
                                        key='WATERPAY')],
                           [sg.Checkbox('Kuu elektripaigaldise hind', pad=(0, 0), default=True,
                                        key='ELECTRICITYPAY')]
                           ], pad=(10, 6))], [
                      sg.Col([[sg.Button('Kopeeri', pad=(6, 0)), sg.Button('Tagasi', pad=(8, 5)), sg.Text('')]],
                             justification='r')]]
    window = sg.Window("Ühistu info", layout_win, modal=True)

    while True:
        event, values = window.read()

        if event == "Tagasi" or event == sg.WIN_CLOSED:
            break

        if event == 'Kopeeri':
            for apar_key in to_check_apar:
                if values[apar_key] is True:
                    to_update[0][apar_key] = ''

            for month_key in to_check_month:
                if values[month_key] is True:
                    to_update[1][month_key] = ''
            break

    window.close()
    return to_update


def pdf_window(apartment_c):
    a_complex = apartment_c.return_info()
    layout_win = [[sg.Col(
        [[sg.Text('Ühistu nimi')], [sg.Text('Ühistu e-mail')],
         [sg.Text('---------------------------')],
         [sg.Text('Tänav')],
         [sg.Text('Linn/Küla')],
         [sg.Text('Vald')], [sg.Text('Maakond')],
         [sg.Text('Postiindeks')], [sg.Text('---------------------------')], [sg.Text('Panga info')],
         [sg.Text('Panga number')]]

        , pad=(0, 0)),
        sg.Col([[sg.InputText(a_complex[0], size=(25, 0), key='ACNAME')],
                [sg.InputText(a_complex[8], size=(25, 0), key='ACEMAIL')],
                [sg.Text('-----------------------------------------------')],
                [sg.InputText(a_complex[1], size=(25, 0), key='ACSTREET')],
                [sg.InputText(a_complex[2], size=(25, 0), key='ACTOWN')],
                [sg.InputText(a_complex[3], size=(25, 0), key='ACMUN')],
                [sg.InputText(a_complex[4], size=(25, 0), key='ACCOUNTY')],
                [sg.InputText(a_complex[5], size=(25, 0), key='ACPCODE')],
                [sg.Text('-----------------------------------------------')],
                [sg.InputText(a_complex[9], size=(25, 0), key='BANKNAME')],
                [sg.InputText(a_complex[10], size=(25, 0), key='BANKNR')],
                ], pad=(0, 12),
               element_justification='c'), sg.Col([[sg.Text('|')] for __ in range(12)]),
        sg.Col(
            [[sg.Col([[sg.Text('Arve valmistaja')],
                      [sg.Text('Telefoninumber')],
                      [sg.Text('---------------------------')],
                      [sg.Text('Arve algusnumber')]],

                     pad=(0, 16), vertical_alignment='top'),
              sg.Col([[sg.InputText(a_complex[6], size=(25, 0), key='ACCREATOR')],
                      [sg.InputText(a_complex[7], size=(25, 0), key='ACPHONE')],
                      [sg.Text('-----------------------------------------------')],
                      [sg.InputText(a_complex[11], size=(10, 0), key='INVOICENR')]],
                     pad=(0, 16), vertical_alignment='top')],
             [sg.Col([[sg.Checkbox('Kasuta arve nimetust', pad=(0, 0), default=False,
                                   key="DGARBAGE")], [sg.Text('')],
                      [sg.Text('')], [sg.Text('')],
                      [sg.Text('Salvesta arve(d):')],
                      [sg.Radio('Üks fail', 'num', default=True, pad=(6, 0)),
                       sg.Radio('Mitu faili', 'num', key='SEPARATEFILE', pad=(20, 0))],
                      ],
                     vertical_alignment='top')]],
            vertical_alignment='top', pad=(0, 0))], [
        sg.Col([[sg.Button('Vormista PDF', pad=(6, 0)), sg.Button('Tagasi', pad=(8, 0)), sg.Text('')], ],
               justification='r')]]
    window = sg.Window("Ühistu info", layout_win, modal=True)

    while True:
        event, values = window.read()
        if event == "Tagasi" or event == sg.WIN_CLOSED:
            save_a_complex = ApartmentComplex(values['ACNAME'], '', values['ACSTREET'],
                                              values['ACTOWN'],
                                              values['ACMUN'], values['ACCOUNTY'],
                                              values['ACPCODE'], values['ACCREATOR'],
                                              values['ACPHONE'],
                                              values['ACEMAIL'], values['BANKNAME'], values['BANKNR'],
                                              values['INVOICENR'])
            break
        if event == 'Vormista PDF':
            save_a_complex = ApartmentComplex(values['ACNAME'], '', values['ACSTREET'],
                                              values['ACTOWN'],
                                              values['ACMUN'], values['ACCOUNTY'],
                                              values['ACPCODE'], values['ACCREATOR'],
                                              values['ACPHONE'],
                                              values['ACEMAIL'], values['BANKNAME'], values['BANKNR'],
                                              values['INVOICENR'])
            try:
                invoicenr = int(values['INVOICENR'])

            except ValueError:
                window.FindElement('INVOICENR').Update(background_color='pink')
                continue

            window.FindElement('INVOICENR').Update(background_color='white')
            cur_month = months_dict[current_month]

            if values['SEPARATEFILE']:
                save_location = sg.popup_get_folder('', no_window=True, initial_folder='/')

            else:
                save_location = sg.popup_get_file('', save_as=True, no_window=True, initial_folder='/',
                                                  file_types=(('PDF', '*.pdf'),))

            if save_location == '':
                continue

            c = PdfGen(invoicenr, save_a_complex, cur_month[1])
            c.genereate_pdf(cur_month[0], save_location, values['SEPARATEFILE'])
            break
    window.close()
    return save_a_complex


# ------ Menu Definition ------ #

menu_def = [['&Fail', ['&Ava', '&Salvesta', '---', '&Salvesta PDF', '---', '&Exit']],
            ['&Edit', ['&Kopeeri kuu']],
            ['&Help', '&About...'], ]

col = [[sg.Text('Osamaks', pad=(5, 0), justification='c'), sg.InputText(size=(7, 0), pad=(0, 0))],
       [sg.Text('Elektripaik', pad=(5, 0)), sg.InputText(size=(7, 0), pad=(0, 0))]]

col_text = [[sg.Text('Osamaks'), sg.Text('Osamaks', pad=(5, 0), justification='c')]]
col_input = [[sg.InputText(size=(7, 0), pad=(0, 0)), sg.InputText(size=(7, 0), pad=(0, 0))]]

col_check = [[sg.Checkbox('Luba muuta kõike', pad=(0, 0), default=True, enable_events=True, disabled=False,
                          key="ALLOWAPART")],
             [sg.Checkbox('Topelt prügi', pad=(0, 0), default=False, enable_events=True, disabled=False,
                          key="DGARBAGE")]]

col_check2 = [[sg.Col([[sg.Text('                               '), sg.Text('Osamaks')],
                       [sg.Text('                               '), sg.Text('Prügi')]], pad=(0, 0), ),
               sg.Col([[sg.InputText(size=(7, 0), key='CONPAY')], [sg.InputText(size=(7, 0), key='GARBAGEPAY')]],
                      pad=(0, 0), )]]

other_column = [
    sg.Col([[sg.Text('Vesi')],
            [sg.Text('Elekter(p)')]], pad=(0, 5),

           vertical_alignment='b'),
    sg.Col([
        [sg.InputText(size=(7, 0), key='WATERPAY')],
        [sg.InputText(size=(7, 0), key="ELECTRICITYPAY")]], pad=(0, 0))]

c_col = (270, 50)
i_col = (150, 50)
col1 = [[sg.Text('Column1')]]
col2 = [[sg.Text('Column1')]]

# layout = [[sg.Menu(menu_def, tearoff=False, pad=(200, 1))],
#           [sg.Combo(months, default_value='Jaanuar', key='MONTH', enable_events=True), sg.Text('Arve täpsem kuupäev'),
#            sg.Input(size=(15, 0), key='DATE')],
#           [text_over_input(headings, ap_info[i], i) for i in range(len(headings))],
#           [[return_columns(col) for col in ap_info]],
#           # [sg.Button('PDF', pad=(10, 0)), sg.Button('JAGA MUUTUMATU INFO'), sg.Button('SALVESTA')],
#           [sg.Col(col_check, size=c_col, vertical_alignment='top'),
#            sg.Col(col_check2, size=c_col, vertical_alignment='top'),
#            sg.Col([other_column], pad=(26, 0), vertical_alignment='top'),
#            sg.Col([[sg.Checkbox('', default=True, enable_events=True, key='PDFCHECK')]], pad=(1, 0),
#                   vertical_alignment='top')]]

layout = [[sg.Menu(menu_def, tearoff=False, pad=(200, 1))],
          [sg.Combo(months, default_value='Jaanuar', key='MONTH', enable_events=True), sg.Text('Arve täpsem kuupäev'),
           sg.Input(size=(15, 0), key='DATE')],
          [text_over_input(i) for i in range(len(headings))],
          # [[return_columns(col) for col in ap_info]],
          # [sg.Button('PDF', pad=(10, 0)), sg.Button('JAGA MUUTUMATU INFO'), sg.Button('SALVESTA')],
          [sg.Col(col_check, size=c_col, vertical_alignment='top'),
           sg.Col(col_check2, size=c_col, vertical_alignment='top'),
           sg.Col([other_column], pad=(26, 0), vertical_alignment='top'),
           sg.Col([[sg.Checkbox('', default=True, enable_events=True, key='PDFCHECK')]], pad=(1, 0),
                  vertical_alignment='top')]]

window = sg.Window('ATAC', layout)

initiate_apartments()

while True:
    event, values = window.read()

    window.refresh()

    if event == "OK" or event == sg.WIN_CLOSED or event == 'Exit':
        break

    if event == 'PDFCHECK':
        for i in range(max_aparts):
            window.FindElement(f'PDF{i}').Update(value=values['PDFCHECK'])

    if values['ALLOWAPART'] is True:
        defualt_enable()

    else:
        defualt_disable()

    # presses the OK button

    if event == 'MONTH':
        update_month(current_month)
        update_water()
        current_month = (months_to_int[values['MONTH']])
        read_month(months_dict[current_month])
        check_january()

    if event == 'Kopeeri kuu':
        update_month_apartment()

    if event == 'Salvesta PDF':
        if check_numbers() is True:
            update_month(current_month)
            update_strings()
            a = pdf_window(apartment_complex)
            if a.return_info()[0] is not None:
                apartment_complex = a

    if event == 'LAE' or event == 'Ava':
        filename = sg.popup_get_file(
            'filename to open', no_window=True, file_types=(('Korteri ühistu fail', '*.kh'),))

        if filename == '':
            continue

        # Remove any content from the inputs
        clear_update()

        loaded_file = pickle.load(open(f'{filename}', "rb"))

        months_dict = loaded_file[0]
        apartment_complex = loaded_file[1]

        read_month(months_dict[current_month])
        window.FindElement('ALLOWAPART').Update(value=False)
        defualt_disable()
        window.refresh()

    if event == 'SALVESTA' or event == 'Salvesta':

        update_month(current_month)

        folder = sg.popup_get_file('', save_as=True, no_window=True, initial_folder='/',
                                   file_types=(('Korteriühistu fail', '*.kh'),))

        if folder == '':
            continue

        pickle.dump([months_dict, apartment_complex], open(f'{folder}', 'wb'))

window.close()
