using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LarpPortal.Classes
{
    static public class cConstants
    {
        static public class Roles
        {
            public const string GUEST_30 = "/30/";

            static public class LARPortal
            {
                public const string DATABASE_OWNER_1 = "/1/";
                public const string DATABASE_ADMINISTRATOR_2 = "/2/";
                public const string APPROVED_USER_9 = "/9/";
                public const string WAITING_APPROVAL_17 = "/17/";
                public const string DENIED_ACCESS_18 = "/18/";
                public const string MASTER_DATABASE_FINANCES_23 = "/23/";
            }

            static public class Campaign
            {
                public const string OWNER_3 = "/3/";
                public const string PLOT_4 = "/4/";
                public const string PLOT_ADJUNCT_14 = "/14/";
                public const string DISCIPLINARY_19 = "/19/";
                public const string FINANCES_22 = "/22/";
                public const string MEDICAL_24 = "/24/";
                public const string GENERAL_MANAGER_28 = "/28/";
            }

            static public class Logistics
            {
                public const string SKILL_UPDATES_5 = "/5/";
                public const string HOUSING_ASSIGNMENT_11 = "/11/";
                public const string NPC_COORDINATOR_12 = "/12/";
                public const string WEBMASTER_MISTRESS_13 = "/13/";
                public const string CP_ASSIGNMENT_15 = "/15/";
                public const string EVENT_CHECK_IN_16 = "/16/";
                public const string MONSTER_MASTER_20 = "/20/";
                public const string ROLE_ASSIGNMENT_21 = "/21/";
                public const string EVENT_SCHEDULING_27 = "/27/";
                public const string PRODUCTION_SKILLS_29 = "/29/";
                public const string WORLD_SETTING_UPDATES_32 = "/32/";
                public const string FOOD_33 = "/33/";
                public const string DONATION_SET_UP_34 = "/34/";
                public const string EVENT_CHECK_OUT_35 = "/35/";
                public const string INVENTORY_PROPS_36 = "/36/";
                public const string EVENT_REGISTRATION_APPROVAL_37 = "/37/";
                public const string DONATION_SET_UP_40 = "/40/";                         //  JB/RP  1/16/2022  Added as part of donations.
            }

            static public class Player
            {
                public const string NPC_PERMANENT_6 = "/6/";
                public const string NPC_EVENT_7 = "/7/";
                public const string PC_8 = "/8/";
                public const string NPC_10 = "/10/";
            }

            static public class Team
            {
                public const string MEMBER_APPROVAL_25 = "/25/";
                public const string MEMBER_26 = "/26/";
                public const string MEMBER_REQUESTED_38 = "/38/";
                public const string MEMBER_INVITED_39 = "/39/";
            }
        }

        static public class SystemMenus
        {
            public const int HOME_1 = 1;
            public const int PLAYER_2 = 2;
            public const int PROFILE_3 = 3;
            static public class Profiles
            {
                public const int ROLES = 4;
                public const int PREFERNCES = 5;
                public const int WAIVER_SCONSENT = 6;
                public const int LARP_RESUME = 7;
                public const int WORK_RESUME = 8;
                public const int MEDICAL_INFO = 9;
                public const int SYSTEM_SECURITY = 10;
                public const int PLAYER_INVENTORY = 11;
                public const int MYCAMPAIGNS = 12;
            }

            public const int CAMPAIGNS_13 = 13;
            static public class Campaigns
            {
                public const int CAMPAIGN_INFO_14 = 14;
                public const int JOIN_A_CAMPAIGN_15 = 15;
                public const int SETUP_16 = 16;

                static public class Setup
                {
                    public const int DEMOGRAPHICS_17 = 17;
                    public const int PLAYER_REQS_18 = 18;
                    public const int CONTACTS_19 = 19;
                    public const int POLICIES_20 = 20;
                    public const int DESCRIPTION_21 = 21;
                    public const int CUSTOM_FIELDS_22 = 22;
                    public const int ASSIGN_ROLES_23 = 23;
                    public const int SYSTEM_MENUS_73 = 73;
                }

                public const int SKILLS_MODIFY_MENU_24 = 24;
                static public class SkillModify
                {
                    public const int SKILL_QUALIFIERS_25 = 25;
                    public const int MODIFY_SKILLS_26 = 26;
                }

                public const int EVENTS_27 = 27;
                static public class Events
                {
                    public const int EVENT_REGISTRATION_28 = 28;
                    public const int REGISTRATION_APPROVAL_29 = 29;
                    public const int SETUP_EVENT_30 = 30;
                    public const int SETUP_DEFAULTS_31 = 31;
                    public const int ASSIGN_HOUSING_32 = 32;
                }

                public const int DONATIONS_33 = 33;
                static public class Donations
                {
                    public const int CLAIM_DONATIONS_34 = 34;
                    public const int SETUP_DONATIONS_35 = 35;
                    public const int RECEIVE_DONATIONS_36 = 36;
                }

                public const int APPROVE_CHARACTER_HISTORIES_37 = 37;
                public const int CHARACTER_BUILD_POINTS_38 = 38;
                public static class CharacterBuildPoints
                {
                    public const int ASSIGN_POINTS_39 = 39;
                    public const int EMAIL_POINTS_40 = 40;
                }

                public const int PELS_41 = 41;
                public static class PELs
                {
                    public const int PEL_APPROVAL_LIST_42 = 42;
                }
                public const int INBETWEEN_GAMES_SKILLS_43 = 43;
                public static class InbetweenGamesSkills
                {
                    public const int INFORMATION_SKILLS_44 = 44;
                }
            }
            public const int CHARACTERS_45 = 45;
            static public class Characters
            {
                public const int CHARACTER_INFO_46 = 46;
                public const int TEAMS_47 = 47;
                static public class Teams
                {
                    public const int CREATE_A_TEAM_48 = 48;
                    public const int JOIN_A_TEAM_49 = 49;
                    public const int MANAGE_A_TEAM_50 = 50;
                }
                public const int SKILLS_51 = 51;
                static public class Skills
                {
                    public const int IN_BETWEEN_SKILLS_52 = 52;
                    public const int INFORMATION_SKILLS_53 = 53;
                }
                public const int HISTORY_54 = 54;
                public const int RELATIONSHIPS_55 = 55;
                public const int PLACES_56 = 56;
                public const int ITEMS_57 = 57;
                public const int GOALS_PREFERENCES_58 = 58;
                public const int CARD_CUSTOMIZATION_59 = 59;
                public const int CARD_ORDER_60 = 60;
                public const int PREVIEW_CHARACTER_CARD_61 = 61;
                public const int ADD_NEW_CHARACTER_62 = 62;
                public const int SKILL_SETS_63 = 63;
                public const int VIEW_PELS_64 = 64;
                public const int REGISTER_FOR_AN_EVENT_65 = 65;
                public const int POINTS_66 = 66;
                static public class Points
                {
                    public const int VIEW_POINTS_67 = 67;
                }
            }
            public const int CALENDAR_68 = 68;
            static public class Calendar
            {
                public const int MONTH_CALENDAR_69 = 69;
                public const int CALENDAR_REPORT_70 = 70;
            }
            public const int REPORTS_71 = 71;
            public const int HOW_TO_VIDEOS_72 = 72;

        }
    }
}