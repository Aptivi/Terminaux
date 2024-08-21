cmake_minimum_required(VERSION 3.25)
project(winchhandler
    VERSION     5.0.0
    DESCRIPTION "Native helper library for Terminaux's console resize listener"
    LANGUAGES   C
)

add_library(winchhandler SHARED
    src/winchhandler.c
)
target_compile_features(winchhandler PUBLIC c_std_99)
target_include_directories(winchhandler PUBLIC include)
