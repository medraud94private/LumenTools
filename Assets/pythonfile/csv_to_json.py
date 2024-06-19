import pandas as pd
import json
import re

# CSV 파일 경로
file_path = 'pythonfile/test.csv'

# CSV 파일 읽기
df = pd.read_csv(file_path, encoding='utf-8-sig')

# 카드 데이터를 저장할 리스트
cards = []

# 특수 조건 파싱 함수
def parse_special_conditions(special_condition):
    conditions = {'highSlot': '', 'midSlot': '', 'lowSlot': ''}
    
    if special_condition:
        # 여러 가지 형식을 고려하여 파싱
        slots = special_condition.split('/')
        for slot in slots:
            slot = slot.strip()
            if '상단' in slot:
                if '방어' in slot:
                    conditions['highSlot'] = '방어'
                elif '회피' in slot:
                    conditions['highSlot'] = '회피'
                elif '상쇄' in slot:
                    conditions['highSlot'] = '상쇄'
                else:
                    conditions['highSlot'] = ''
            elif '중단' in slot:
                if '방어' in slot:
                    conditions['midSlot'] = '방어'
                elif '회피' in slot:
                    conditions['midSlot'] = '회피'
                elif '상쇄' in slot:
                    conditions['midSlot'] = '상쇄'
                else:
                    conditions['midSlot'] = ''
            elif '하단' in slot:
                if '방어' in slot:
                    conditions['lowSlot'] = '방어'
                elif '회피' in slot:
                    conditions['lowSlot'] = '회피'
                elif '상쇄' in slot:
                    conditions['lowSlot'] = '상쇄'
                else:
                    conditions['lowSlot'] = ''
    
    return conditions

# 모든 데이터를 돌면서 파싱
for index, row in df.iterrows():
    conditions = parse_special_conditions(row['위치']) if pd.notna(row['위치']) and row['위치'] != '-' else {'highSlot': '', 'midSlot': '', 'lowSlot': ''}
    card = {
        'number': row['번호'],
        'name': row['이름'] if pd.notna(row['이름']) and row['이름'] != '-' else '',
        'character': row['캐릭터'] if pd.notna(row['캐릭터']) and row['캐릭터'] != '-' else '',
        'cardType': row['타입'] if pd.notna(row['타입']) and row['타입'] != '-' else '',
        'speed': int(row['속도']) if pd.notna(row['속도']) and row['속도'] != '-' else None,
        'attackLine': row['위치'] if pd.notna(row['위치']) and row['위치'] != '-' else '',
        'attackPower': int(row['데미지']) if pd.notna(row['데미지']) and row['데미지'] != '-' else None,
        'limbType': row['손발판정'] if pd.notna(row['손발판정']) and row['손발판정'] != '-' else '',
        'specialCondition': row['특수판정'] if pd.notna(row['특수판정']) and row['특수판정'] != '-' else '',
        'hitFP': row['히트판정'] if pd.notna(row['히트판정']) and row['히트판정'] != '-' else '',
        'counterFP': row['카운터판정'] if pd.notna(row['카운터판정']) and row['카운터판정'] != '-' else '',
        'guardFP': row['가드판정'] if pd.notna(row['가드판정']) and row['가드판정'] != '-' else '',
        'effects': row['효과'] if pd.notna(row['효과']) and row['효과'] != '-' else '',
        'highSlot': conditions['highSlot'],
        'midSlot': conditions['midSlot'],
        'lowSlot': conditions['lowSlot']
    }
    cards.append(card)

# JSON 파일로 저장
json_path = 'data/card_database.json'
with open(json_path, 'w', encoding='utf-8') as json_file:
    json.dump(cards, json_file, ensure_ascii=False, indent=4)

json_path
