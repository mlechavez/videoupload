/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp'),
    plugins = require('gulp-load-plugins')(),
    gulpIf = require('gulp-if'),
    runSequence = require('run-sequence');

gulp.task('sass', function () {
    gulp.src('assets/sass/**/*.scss')
        .pipe(plugins.sourcemaps.init())
        .pipe(plugins.sass())
        .pipe(plugins.autoprefixer())
        .pipe(plugins.sourcemaps.write())
        .pipe(gulp.dest('assets/css'))
        .pipe(plugins.livereload());
});

gulp.task('watch', function () {
    plugins.livereload.listen();
    gulp.watch('assets/sass/**/*.scss', ['sass']);
    gulp.watch('assets/js/**/*.js', ['js']);
});

gulp.task('js', function () {
    return gulp.src(['assets/js/**/*.js', '!assets/js/lib/**/*.js'])
        .pipe(gulp.dest('dist/js'));
});

gulp.task('images', function () {
    return gulp.src('assets/images/**/*.+(png|jpg|jpeg|gif|svg')
        .pipe(gulp.dest('dist/images'));
});

gulp.task('fonts', function () {
    return gulp.src('assets/fonts/**/*')
        .pipe(gulp.dest('dist/fonts'));
});


gulp.task('default', function (callBack) {
    runSequence(['sass', 'watch', 'js', 'images', 'fonts'], callBack);
});

gulp.task('optimize', function () {
    //when this runs, don't forget 
    //to change the '../..' path to '~' in _layout.cshtml

    gulp.src('./**/*.cshtml')
        .pipe(plugins.useref())
        .pipe(gulpIf('*.js', plugins.uglify()))
        .pipe(gulpIf('*.css', plugins.cssnano()))
        .pipe(gulp.dest(function (data) {
            return data.base;
    }));
});