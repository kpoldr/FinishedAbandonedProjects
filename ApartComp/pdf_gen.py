from reportlab.lib import colors
from reportlab.lib.pagesizes import LETTER
from reportlab.pdfbase import pdfmetrics
from reportlab.pdfbase.pdfmetrics import registerFontFamily
from reportlab.pdfbase.ttfonts import TTFont
from reportlab.platypus import SimpleDocTemplate, Paragraph, Table, Spacer, TableStyle, ListFlowable, ListItem, \
    PageBreak
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle, ListStyle
from apartment import Apartment
from month import Month
from apartment_complex import ApartmentComplex

pdfmetrics.registerFont(TTFont('Arial', 'arial.ttf'))
pdfmetrics.registerFont(TTFont('Arial-Bold', 'arialbd.ttf'))
pdfmetrics.registerFont(TTFont('Arial-Italic', 'ariali.ttf'))
pdfmetrics.registerFont(TTFont('Arial-BoldItalic', 'arialbi.ttf'))
registerFontFamily('Arial', normal='Arial', bold='Arial-Bold', italic='Arial-Italic', boldItalic='Arial-BoldItalic')

doc = SimpleDocTemplate("form_letter.pdf", pagesize=LETTER,
                        rightMargin=72, leftMargin=80,
                        topMargin=60, bottomMargin=18)
Story = []

# ------------ Style Definition ------------ #

style = getSampleStyleSheet()

style.add(ParagraphStyle('title_invoice',
                         fontName="Arial-Bold",
                         fontSize=13,
                         alignment=1,
                         spaceAfter=3,
                         leading=13))

style.add(ParagraphStyle('normal',
                         fontName="Arial",
                         fontSize=10,
                         spaceAfter=1,
                         leading=13
                         ))

style.add(ParagraphStyle('date',
                         parent=style['normal'],
                         alignment=1))

style.add(ParagraphStyle('address',
                         parent=style['normal'],
                         fontSize=10,
                         leading=6))

style.add(ParagraphStyle('normal_11',
                         parent=style['normal'],
                         fontSize=11
                         ))

style.add(ParagraphStyle('footer',
                         parent=style['address'],
                         leading=4,
                         fontSize=8))

style.add(ParagraphStyle('indent',
                         parent=style['address'],
                         leftIndent=40))

style.add(ParagraphStyle('address_r',
                         parent=style['address'],
                         leftIndent=20))

style.add(ParagraphStyle('indent_r',
                         parent=style['address'],
                         leftIndent=60))

text_si = ParagraphStyle('indent',
                         parent=style['address'],
                         leftIndent=20)

text_si_i = ParagraphStyle('indent',
                           parent=style['address'],
                           leftIndent=60)

class PdfGen:

    def __init__(self, nr, apartment_c, month):
        self.Story = []
        self.nr = int(nr)
        self.apartment_c = apartment_c
        self.month = month

    def title_build(self):
        nr_str = f'Arve {self.nr}'
        if self.nr < 10:
            nr_str = f'Arve 00{self.nr}'
        elif self.nr < 100:
            nr_str = f'Arve 0{self.nr}'
        self.Story.append(Paragraph(nr_str, style['title_invoice']))
        self.Story.append(Paragraph(f'Kuupäev: {self.month.date}', style['date']))
        self.Story.append(Spacer(1, 24))

    def address_build(self, cur_apar):
        tbl_data2 = [
            [Paragraph(f'Arve saaja: <b> {cur_apar.owner}</b>', style['address']),
             Paragraph(f'Arve esitaja:<b> {self.apartment_c.name}</b>', style['address_r'])],
            [Paragraph(f'Aadress: {self.apartment_c.street}-{cur_apar.nr}', style['address']),
             Paragraph(f'Aadress: {self.apartment_c.street}', style['address_r'])],
            [Paragraph(self.apartment_c.town, style['indent']), Paragraph(self.apartment_c.town, style['indent_r'])],
            [Paragraph(self.apartment_c.municipality, style['indent']),
             Paragraph(self.apartment_c.municipality, style['indent_r'])],
            [Paragraph(f'{self.apartment_c.postal_code}  {self.apartment_c.county}', style['address']),
             Paragraph(f'{self.apartment_c.postal_code}  {self.apartment_c.county}', style['address_r'])],
            [Paragraph('', style['address']), Paragraph('<br /> <br /> Reg. Nr 802922270', style['address_r'])],
            [Paragraph('', style['address']), Paragraph(f'<br /> <br /> {self.apartment_c.bank_name}', style['address_r'])],
            [Paragraph('', style['address']), Paragraph(self.apartment_c.bank_nr, style['address_r'])]
        ]

        tbl2 = Table(tbl_data2)

        self.Story.append(tbl2)
        self.Story.append(Spacer(1, 20))

    def table_build(self, cur_apar):
        people = str(cur_apar.people)

        debt = 0.00
        ad_pay = 0.00

        if float(cur_apar.pay_debt) <= 0:
            debt = cur_apar.pay_debt
        else:
            ad_pay = cur_apar.pay_debt

        if self.month.double_m is True:
            people += '(*2)'

        info_tbl_data = [['Kululiik', '   Periood   ', '   Kogus   ', 'Ühik', 'Ühiku Hind', 'Summa    ', '\n'],
                         ['Vesi/kanalisatsioon                ', self.month.name, f'{cur_apar.cal_water_dif()}',
                          'm³', f'{self.month.water:.3f}', f'{cur_apar.cal_water_pay():.2f}'],
                         ['Prügi', self.month.name, people, 'in', f'{self.month.garbage:.2f}',
                          f'{cur_apar.cal_garbage_pay():.2f}'],
                         ['Osamaks', self.month.name, f'{cur_apar.size:.2f}', 'm²', f'{self.month.con_pay}',
                          f'{cur_apar.cal_con_pay():.2f}'],
                         ['Elektripaigaldise \nkäidukontrol', self.month.name, '1', 'krt',
                          '1', f'{self.month.electricity_p:.2f}'],
                         ['\n', '', 'Kokku', '', '', f'{cur_apar.cal_pay():.2f}'],
                         ['', '', f'Võlg (seisuga {self.month.date})', '', 'V', f'{debt:.2f}'],
                         ['', '', f'Ettemaks (seisuga {self.month.date})', '', '', f'{ad_pay:.2f}'],
                         ['', '', f'Laekunud (seisuga {self.month.date})', '', '', f'{cur_apar.received:.2f}'],
                         ['', '', f'Viivis (seisuga {self.month.date})', '', '', f'{cur_apar.fine:.2f}'],
                         ['\n', '', 'Tasumisele kulub', '', '', f'{cur_apar.cal_final_pay():.2f}']
                         ]

        into_tbl = Table(info_tbl_data)

        into_tbl.setStyle(
            TableStyle([('VALIGN', (0, 0), (-1, -1), 'MIDDLE'), ('ALIGN', (1, 1), (1, 4), 'CENTER'),
                        ('INNERGRID', (0, 0), (5, 4), 0.25, colors.black),
                        ('INNERGRID', (2, 5), (5, 10), 0.25, colors.black),
                        ('LINEABOVE', (0, 0), (5, 0), 0.25, colors.black),
                        ('LINEBELOW', (0, 4), (5, 4), 0.25, colors.black),
                        ('LINEBELOW', (0, 10), (5, 10), 0.25, colors.black),
                        ('FONT', (0, 0), (-1, 0), 'Arial-Bold'), ('FONT', (2, 5), (-1, 5), 'Arial-Bold'),
                        ('FONT', (2, 10), (-1, 10), 'Arial-Bold'),
                        ('SPAN', (2, 5), (4, 5)), ('SPAN', (2, 6), (4, 6)), ('SPAN', (2, 7), (4, 7)),
                        ('SPAN', (2, 8), (4, 8)),
                        ('SPAN', (2, 9), (4, 9)), ('SPAN', (2, 10), (4, 10))]), )

        self.Story.append(into_tbl)

    def after_table_build(self, cur_apar):

        self.Story.append(Spacer(1, 5))

        self.Story.append(Paragraph('Maksetähtaeg:', style['normal']))

        self.Story.append(Spacer(1, 15))

        self.Story.append(Paragraph(f'Algnäit:    {cur_apar.water_old:.3f}', style['normal']))
        self.Story.append(Paragraph(f'Lõppnäit: {cur_apar.water_new:.3f}', style['normal']))

        self.Story.append(Spacer(1, 25))

        self.Story.append(Paragraph(f'Koostaja: {self.apartment_c.creator}', style['normal']))
        self.Story.append(Paragraph(f'Telefon {self.apartment_c.phone}', style['normal']))

        self.Story.append(Spacer(1, 10))

    def build_other_info(self):
        flow_list = ListStyle('epic', bulletFontName='Arial', bulletFontSize=10)

        my_list = ListFlowable([
            ListItem(Paragraph('on alates 01.01.2011 ühistu osamaks <b>0.10 €</b> m²', style['normal_11']),
                     leftIndent=40, value='1.',
                     ),
            ListItem(Paragraph('alates 01.01.2011 arvestatakse  3 kuud maksmata arve(te)lt <b>viivist 0.07 %</b> <br />'
                               'päevas kuni kõikide tasude täieliku laekumiseni.', style['normal_11']),
                     leftIndent=40, value='2.'),
        ], style=flow_list, bulletType='bullet')

        self.Story.append(
            Paragraph('&nbsp Korteriühistu Kase tee 5 üldkoosoleku otsusega 08.detsembrist 2010:', style['normal_11']))
        self.Story.append(Spacer(1, 10))
        self.Story.append(my_list)
        self.Story.append(Spacer(1, 10))

    def build_footer(self):

        tbl_data = [
            [Paragraph(f'{self.apartment_c.email}', style['footer']), Paragraph('', style['address']),
             Paragraph('', style['address']),
             Paragraph(f'{self.apartment_c.name}', style['footer'])],
            [Paragraph('', style['address']), Paragraph('', style['address']), Paragraph('', style['address']),
             Paragraph(f'<br /> {self.apartment_c.street}, {self.apartment_c.town}', style['footer'])],
            [Paragraph('', style['address']), Paragraph('', style['address']), Paragraph('', style['address']),
             Paragraph(f'{self.apartment_c.municipality}', style['footer'])],
            [Paragraph('', style['address']), Paragraph('', style['address']), Paragraph('', style['address']),
             Paragraph(f'{self.apartment_c.postal_code} {self.apartment_c.county}', style['footer'])],

        ]

        tbl_foot = Table(tbl_data)
        self.Story.append(tbl_foot)

    def genereate_pdf(self, apartments, location, separate):
        doc = SimpleDocTemplate(location, pagesize=LETTER,
                                rightMargin=72, leftMargin=80,
                                topMargin=60, bottomMargin=18)

        for i in range(len(apartments)):
            if apartments[i].pdf:
                self.title_build()
                self.address_build(apartments[i])
                self.table_build(apartments[i])
                self.after_table_build(apartments[i])
                self.build_other_info()
                self.build_footer()
                if separate:
                    doc = SimpleDocTemplate(f'{location}/Arve_{i}_{apartments[i].owner}.pdf', pagesize=LETTER,
                                            rightMargin=72, leftMargin=80,
                                            topMargin=60, bottomMargin=18)

                    doc.build(self.Story)
                else:
                    self.Story.append(PageBreak())
            self.nr += 1

        if not separate:
            doc.build(self.Story)

# cur_month = Month('Aprill', '15.04.2021', 0.20, 0.73, 3.996, 3.56, True)
# apar = Apartment('16', 72.1, 'Anu Tamm', 3, 1167, 1168, 0, cur_month)
# apar2 = Apartment('11', 87.6, 'Peeter Vadim', 5, 2139, 2142, 300, cur_month)
# aparts = [apar, apar2]
# apar_c = ApartmentComplex('Korteriühistu Kase Tee 5', 18, 'Kase Tee 5', 'Pisisaare', 'Pajusi Vald', 'Jõgevamaa',
#                           '48216', 'Anu Tamm', '56956602', 'kasetee5@gmail.com')
#
# c = PdfGen('Arve 123', apar_c, cur_month)
# c.genereate_pdf(aparts, 'form_letter2.pdf')
