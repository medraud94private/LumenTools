import pandas as pd
import json

# 엑셀 파일 경로
file_path = 'pythonfile/루멘콘덴서 풀 데이터베이스.xlsx'

# 엑셀 파일의 모든 시트 읽기
xls = pd.ExcelFile(file_path)

# 카드 데이터를 저장할 리스트
cards = []

# 모든 시트를 돌면서 데이터 파싱
for sheet_name in xls.sheet_names:
    df = pd.read_excel(file_path, sheet_name=sheet_name)
    for index, row in df.iterrows():
        card = {
            'name': row['이름'] if '이름' in row and pd.notna(row['이름']) and row['이름'] != '-' else '',
            'character': row['캐릭터'] if '캐릭터' in row and pd.notna(row['캐릭터']) and row['캐릭터'] != '-' else '',
            'cardType': row['타입'] if '타입' in row and pd.notna(row['타입']) and row['타입'] != '-' else '',
            'speed': int(row['속도']) if '속도' in row and pd.notna(row['속도']) and row['속도'] != '-' else None,
            'attackLine': row['위치'] if '위치' in row and pd.notna(row['위치']) and row['위치'] != '-' else '',
            'attackPower': int(row['데미지']) if '데미지' in row and pd.notna(row['데미지']) and row['데미지'] != '-' else None,
            'limbType': row['손발판정'] if '손발판정' in row and pd.notna(row['손발판정']) and row['손발판정'] != '-' else '',
            'specialCondition': row['특수판정'] if '특수판정' in row and pd.notna(row['특수판정']) and row['특수판정'] != '-' else '',
            'hitFP': row['히트판정'] if '히트판정' in row and pd.notna(row['히트판정']) and row['히트판정'] != '-' else '',
            'counterFP': row['카운터판정'] if '카운터판정' in row and pd.notna(row['카운터판정']) and row['카운터판정'] != '-' else '',
            'guardFP': row['가드판정'] if '가드판정' in row and pd.notna(row['가드판정']) and row['가드판정'] != '-' else '',
            'effects': row['효과'] if '효과' in row and pd.notna(row['효과']) and row['효과'] != '-' else ''
        }
        cards.append(card)

# JSON 파일로 저장
with open('card_database.json', 'w', encoding='utf-8') as json_file:
    json.dump(cards, json_file, ensure_ascii=False, indent=4)

print("카드 데이터가 'card_database.json' 파일에 저장되었습니다.")
