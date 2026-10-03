#!/bin/bash

# Log parent posts
python log_encounter.py "https://www.x.com/status/1" "https://www.x.com/status/1" "2026-09-29 12:42-41" 8 "Manager" 90 "Kroger closes Ohio stores" --run_name "Test"
python log_encounter.py "https://www.x.com/status/3" "https://www.x.com/status/3" "2026-09-30 01:22:01" 5 "Employee" 65 "Employee complaint of pest infestation" --run_name "Test"
python log_encounter.py "https://www.x.com/status/5" "https://www.x.com/status/5" "2026-09-30 05:28:42" 3 "Employee" 80 "Employee complaint of fridge issue" --run_name "Test"

# Log comments
python log_encounter.py "https://www.x.com/status/2" "https://www.x.com/status/1" "2026-09-29 13:13:13" 2 "Customer" 80 "Customer reaction to store closure" --run_name "Test"
python log_encounter.py "https://www.x.com/status/4" "https://www.x.com/status/1" "2026-09-29 13:29:57" 3 "Customer" 70 "Customer saying LIDL is better" --run_name "Test"
python log_encounter.py "https://www.x.com/status/6" "https://www.x.com/status/5" "2026-09-30 06:55:20" 2 "Employee" 80 "Employee notes fridge issue in other stores" --run_name "Test"
python log_encounter.py "https://www.x.com/status/7" "https://www.x.com/status/5" "2026-09-30 19:26:18" 2 "Customer" 90 "Customer notes fridge issue led to purchase of spoiled product" --run_name "Test"
python log_encounter.py "https://www.x.com/status/9" "https://www.x.com/status/8" "2026-10-01 09:49:50" 5 "Customer" 90 "Customer angry at Kroger manager termination" --run_name "Test"
python log_encounter.py "https://www.x.com/status/10" "https://www.x.com/status/8" "2026-10-1 10:01:22" 4 "Customer" 90 "Customer claiming to boycott Kroger" --run_name "Test"

# Log situations
python log_situation.py "2026_09_Store_Closure" --situation_description "Kroger CEO annouces 15 store closures across Ohio" --run_name "Test"
python log_situation.py "2026_09_Arkansas_Pest_Issue" --situation_description "Employee notices new pest infestation in Arkansas Kroger locations" --run_name "Test"
python log_situation.py "2026_09_Fridge_Issue" --situation_description "Widespread fridge failures across Kroger locations" --run_name "Test"
python log_situation.py "LIDL Competition" --situation_description "LIDL is eating Kroger's customer base" --run_name "Test"
python log_situation.py "2026_09_Wrongful_Manager_Termination" --situation_description "Kroger manager was fired after standing up for fellow employees" --run_name "Test"

# Correlate posts to appropriate situations
python log_encounter_situation.py "https://www.x.com/status/1" "2026_09_Store_Closure" --run_name "Test"
python log_encounter_situation.py "https://www.x.com/status/2" "2026_09_Store_Closure" --run_name "Test"
python log_encounter_situation.py "https://www.x.com/status/3" "2026_09_Arkansas_Pest_Issue" --run_name "Test"
python log_encounter_situation.py "https://www.x.com/status/4" "LIDL Competition" --run_name "Test"
python log_encounter_situation.py "https://www.x.com/status/4" "2026_09_Store_Closure" --run_name "Test"
python log_encounter_situation.py "https://www.x.com/status/5" "2026_09_Fridge_Issue" --run_name "Test"
python log_encounter_situation.py "https://www.x.com/status/6" "2026_09_Fridge_Issue" --run_name "Test"
python log_encounter_situation.py "https://www.x.com/status/7" "2026_09_Fridge_Issue" --run_name "Test"
python log_encounter_situation.py "https://www.x.com/status/8" "2026_09_Wrongful_Manager_Termination" --run_name "Test"
python log_encounter_situation.py "https://www.x.com/status/9" "2026_09_Wrongful_Manager_Termination" --run_name "Test"
python log_encounter_situation.py "https://www.x.com/status/10" "2026_09_Wrongful_Manager_Termination" --run_name "Test"

# Log sources (only a few, for brevity's sake)
python log_sources.py "https://www.x.com/status/1" "Mananger" "A" "Statement from CEO Greg Foran" "A" "D" "F" "D" "F" --run_name "Test"
python log_sources.py "https://www.x.com/status/2" "Customer" "C" "Negative reaction about shopping habits" "F" "F" "C" "F" "F" --run_name "Test"
python log_sources.py "https://www.x.com/status/3" "Employee" "B" "First-hand testimony" "F" "B" "D" "E" "F" --run_name "Test"

# Log visibility (only a few, for brevity's sake)
python log_visibility.py "https://www.x.com/status/1" 1250000 1300 800 1500 --run_name "Test"
python log_visibility.py "https://www.x.com/status/2" 1200 46 0 2 --run_name "Test"
python log_visibility.py "https://www.x.com/status/3" 11900 231 6 19 --run_name "Test"

# Log business implications (only a few, for brevity's sake)
python log_business.py "https://www.x.com/status/1" 10 "A" "Downsizing news from CEO" 5 "B" "According to filed 10-Qs and 10-Ks, Kroger scales revenue through number of locations" -2 "B" "Downsizing is likely bad for future revenue" --run_name "Test"

