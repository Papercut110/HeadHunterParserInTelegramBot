# HeadHunterParserInTelegramBot

This program is a test version of the headhunter site parser. We parse the results of the query for the vacancy we are interested in. In the parameters of the Start method, we pass the frequency of updates of the request for the profession we are interested in, as well as the request text. (we take into account url characters) By default, the program saves the json file to the root of the C drive and compares it with new vacancies for your profession, if any, they are sent to you by telegram bot, and the json file itself is updated. For the work of the bot's telegrams, you need to register a token in the token field and the chat id, in which the bot will drop information about vacancies. An example of the id definition is commented out in the TeleBot class.
