using MongoDB.Bson;

namespace Executer.Enums
{
    //Version: 20181119 1802

    public enum  StateMaschine : int
    {
        //action numeric definitions:
        //entry of action :: wait/ask status of action :: action succesful? :: error in action?

        init = 0,

        buy0 = 100,
        buy1 = 101,
        buy2 = 102,
        buy3 = 103,
        buy4 = 104,

        buy = 100,
        buy_price = 105,
        buy_state = 110,
        buy_querydb_set_open_slots = 120,
        buy_error_cleanup_some_unsold_orders = 121,
        buy_api_send_order = 135,
        long_close_signal = 140,
        long_check_any_open = 142,
        buy_modify_open_order = 170,


        
        open_long_success = 600,
        find_querydb_open_longs = 120,
        buy_api_results_after_trade_success = 180,
        buy_success = 1000,
        open_long_success_overtaken = 650,
        buy_completed_fire_sell = 190,
        fire_sell_completes_set_ids = 195,
        close_long_success = 6000,

        open_short_success = 700,
        find_querydb_open_shorts = 220,
        sell_api_results_after_trade_success = 280,
        sell_success = 2000,
        open_short_success_overtaken = 750,
        sell_completed_fire_buy = 290,
        fire_buy_completes_set_ids = 295,
        close_short_success = 7000,








        buy_price_higer_then_sell = 1001,
        long_closed_now_short_begins = 1001,
        long_proceed_with_position = 1002,
        buy_price_lower_then_sell = 1010,
        buy_error = 1111,
        buy_error_EachTradeMaxAsUSDT_cant_be_zero = 1112,
        buy_error_insufficient_balance_1 = 1113,
        buy_error_insufficient_balance_in_catch = 1114,
        buy_error_canceled = 1115,
        buy_error_order_not_set = 1116,
        buy_error_EachTradeMaxAsUSDT_cant_be_minusOne = 1117,
        buy_error_ticker = 1118,
        buy_error_modify = 1119,
        buy_error_already_buyorder_active = 1120,
        buy_error_read_api_short = 1242,
        buy_error_cancel_api_short = 1243,
        buy_error_cancel_order = 1244,



        sell0 = 200,
        sell1 = 201,
        sell2 = 202,
        sell3 = 203,
        sell4 = 204,

        sell = 200,
        sell_force = 201,
        sell_amount = 205,
        sell_price = 210,
        sell_price_force = 215,
        sell_queryapi_find_elements_to_be_sold = 216,
        //sell_querydb_find_elements_to_be_sold = 220,
        sell_querydb_find_elements_to_be_sold_force = 225,
        sell_querydb_set_back_elements_not_to_be_sold = 230,
        sell_api_send_order = 235,
        short_close_signal = 240,
        short_check_any_open = 242,
        short_cancel_order = 243,
        sell_querydb_set_open_slots = 250,
        sell_api_sent_order_wait = 260,
        sell_modify_open_order = 270,
        //sell_completed_set_orderids_fom_buy = 290,


        //buy_overtaken_from_sell_and_bought = 750,




        short_proceed_with_position = 2002,
        sell_error_EachTradeMaxAsUSDT_cant_be_zero = 2112,
        sell_error = 2222,
        sell_error_force = 2225,
        sell_timeout_error = 2223,
        sell_set_open_slot_error = 2224,
        sell_error_in_catch_1 = 2226,
        sell_error_in_catch_2 = 2227,
        sell_error_in_catch_3 = 2228,
        sell_error_DocsSell_null = 2229,
        sell_error_DocsSell_count_zero = 2230,
        sell_error_FindAmountSell = 2231,
        sell_error_SellPriceForOrder = 2232,
        sell_error_result_1 = 2233,
        sell_error_result_2 = 2234,
        sell_error_in_catch_4 = 2235,
        sell_error_modify = 2236,
        sell_error_insufficient_balance_catch_1 = 2237,
        sell_error_insufficient_balance_catch_2 = 2238,
        sell_error_ticker = 2239,
        sell_error_apicall = 2240,
        sell_error_already_sellorder_active = 2241,
        sell_error_read_api_short = 2242,
        sell_error_cancel_api_short = 2243,
        sell_error_cancel_order = 2244,
        sell_error_unexpected_signal = 2245,


        st_sell_error_DocsSell_null = 2246,
        st_sell_error_DocsSell_count_zero = 2247,
        st_sell_error_FindAmountSell = 2248,

        executersettingupdate = 300,
        executersettingupdatefinised = 3001,
        executersettingupdateerror = 9003,

        executer_check_stoploss = 400,
        executer_check_stoploss_deactivated = 9006,
        executer_sell_stoploss = 401,
        st_sell_overtaken_from_buy_and_sold = 4000,



        error_at_the_beginning_of_app = 9000,
        error_at_the_beginning_of_app_init = 9001,
        skip_at_the_beginning_of_app = 9001,
        cancel_at_the_beginning_of_app = 9002,
        error_deserializeObject = 9003,
        error_coin_offline = 9004,
        error_coin_allow_trade_when_sell_trend = 9005,
        error_api_call = 9997,
        work_buy_not_set = 9998,
        work_sell_not_set = 9999,
        not_yet_implemented = 99999,

        error_filter_violation = 10000,
        error_filter_not_yet_implemented = 10001,
        error_LastSTVPTsignal_not_yet_set = 10002
    }
}